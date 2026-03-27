using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.Fakes.Builders;
using StruttonTechnologies.Core.Identity.Orchestration.JwtTokens;
using StruttonTechnologies.Core.Identity.Stub.Factories;

namespace StruttonTechnologies.Core.Identity.Orchestration.Tests.Services
{
    /// <summary>
    /// Unit tests for <see cref="TokenOrchestration{TKey}"/>.
    /// Validates token generation, expiration, revocation, and malformed token handling.
    /// </summary>
    public class TokenOrchestrationTests
    {
        private readonly StubRevocableTokenManager _jwtManager;
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

            string token = await _service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken);

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
            string token = await _service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken);

            await _jwtManager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            bool isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(token, TestContext.Current.CancellationToken);
            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsTokenRevoked_ReturnsFalse_ForUnrevokedToken()
        {
            ClaimsPrincipal principal = TestClaimsPrincipalBuilder.CreateDefault();
            string token = await _service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken);

            bool isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(token, TestContext.Current.CancellationToken);
            Assert.False(isRevoked);
        }

        [Fact]
        public async Task IsTokenRevoked_ReturnsFalse_ForMalformedToken()
        {
            string malformedToken = "not.a.valid.jwt";

            bool isRevoked = await _jwtManager.IsAccessTokenRevokedAsync(malformedToken, TestContext.Current.CancellationToken);
            Assert.False(isRevoked);
        }
    }
}
