using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ST.Core.Identity.Application.Services.JwtTokens;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Fakes.Builders;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Orchestration.JwtTokens;
using ST.Core.Identity.Stub.Factories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Orchestration.Tests.Services
{
    /// <summary>
    /// Unit tests for <see cref="TokenOrchestration{TKey}"/>.
    /// Validates token generation, expiration, revocation, and malformed token handling.
    /// </summary>
    public class TokenOrchestrationTests
    {
        private readonly ILogger<TokenOrchestration<Guid>> _logger = MockLoggerFactory.Create<TokenOrchestration<Guid>>();
        private readonly IRevocableTokenManager _jwtManager;
        private readonly TokenOrchestration<Guid> _service;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenOrchestrationTests"/> using default token options and stubbed token manager.
        /// </summary>
        public TokenOrchestrationTests()
        {
            var options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            _jwtManager = new StubRevocableTokenManager();
            _service = new TokenOrchestration<Guid>(options, _logger, _jwtManager);
        }

        [Fact]
        public async Task GenerateToken_ReturnsValidJwt()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();

            var token = await _service.GenerateTokenAsync(principal);

            Assert.False(string.IsNullOrWhiteSpace(token));
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            Assert.Equal("issuer", jwt.Issuer);
            Assert.Equal("audience", jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "StubUser");
            Assert.Contains(jwt.Claims, c => c.Type == "jti");
        }

        [Fact]
        public void GetExpirationTime_ReturnsFutureTimestamp()
        {
            var expiration = _service.GetExpirationTime();

            Assert.True(expiration > DateTime.UtcNow);
            Assert.True(expiration <= DateTime.UtcNow.AddMinutes(60));
        }

        [Fact]
        public async Task RevokeToken_MarksTokenAsRevoked()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();
            var token = await _service.GenerateTokenAsync(principal);

            await _jwtManager.RevokeAccessTokenAsync(token, CancellationToken.None);

            var isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(token, CancellationToken.None);
            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsTokenRevoked_ReturnsFalse_ForUnrevokedToken()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();
            var token = await _service.GenerateTokenAsync(principal);

            var isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(token, CancellationToken.None);
            Assert.False(isRevoked);
        }

        [Fact]
        public async Task IsTokenRevoked_ReturnsFalse_ForMalformedToken()
        {
            var malformedToken = "not.a.valid.jwt";

            var isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(malformedToken, CancellationToken.None);
            Assert.False(isRevoked);
        }
    }
}