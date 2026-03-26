using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using StruttonTechnologies.Core.Identity.Dispatch.Services.JwtTokens;
using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.Fakes.Builders;
using StruttonTechnologies.Core.Identity.Fakes.Factories;
using StruttonTechnologies.Core.Identity.Stub.Factories;

namespace StruttonTechnologies.Core.Identity.Orchestration.Tests.Services
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
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            _jwtManager = new StubRevocableTokenManager();
            _service = new TokenOrchestration<Guid>(options, _jwtManager);
        }

        [Fact]
        public async Task GenerateToken_ReturnsValidJwt()
        {
            ClaimsPrincipal principal = TestClaimsPrincipalBuilder.CreateDefault();

            string token = await _service.GenerateTokenAsync(principal);

            Assert.False(string.IsNullOrWhiteSpace(token));
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            Assert.Equal("issuer", jwt.Issuer);
            Assert.Equal("audience", jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "StubUser");
            Assert.Contains(jwt.Claims, c => c.Type == "jti");
        }

        [Fact]
        public void GetExpirationTime_ReturnsFutureTimestamp()
        {
            DateTime expiration = _service.GetExpirationTime();

            Assert.True(expiration > DateTime.UtcNow);
            Assert.True(expiration <= DateTime.UtcNow.AddMinutes(60));
        }

        [Fact]
        public async Task RevokeToken_MarksTokenAsRevoked()
        {
            ClaimsPrincipal principal = TestClaimsPrincipalBuilder.CreateDefault();
            string token = await _service.GenerateTokenAsync(principal);

            await _jwtManager.RevokeAccessTokenAsync(token, CancellationToken.None);

            bool isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(token, CancellationToken.None);
            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsTokenRevoked_ReturnsFalse_ForUnrevokedToken()
        {
            ClaimsPrincipal principal = TestClaimsPrincipalBuilder.CreateDefault();
            string token = await _service.GenerateTokenAsync(principal);

            bool isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(token, CancellationToken.None);
            Assert.False(isRevoked);
        }

        [Fact]
        public async Task IsTokenRevoked_ReturnsFalse_ForMalformedToken()
        {
            string malformedToken = "not.a.valid.jwt";

            bool isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(malformedToken, CancellationToken.None);
            Assert.False(isRevoked);
        }
    }
}
