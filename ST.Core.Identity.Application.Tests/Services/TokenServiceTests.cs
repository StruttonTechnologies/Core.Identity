using Microsoft.Extensions.Logging;
using ST.Core.Identity.Application.Services.JwtTokens;
using ST.Core.Identity.Fakes.Builders;
using ST.Core.Identity.Fakes.Factories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace ST.Core.Identity.Application.Tests.Services
{
    /// <summary>
    /// Unit tests for <see cref="TokenService"/>.
    /// Validates token generation, expiration, revocation, and malformed token handling.
    /// </summary>
    public class TokenServiceTests
    {
        private readonly ILogger<TokenService> _logger = MockLoggerFactory.Create<TokenService>();
        private readonly TokenService _service;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenServiceTests"/> using default token options and a mock logger.
        /// </summary>
        public TokenServiceTests()
        {
            _service = new TokenService(JwtTokenOptionsFactory.CreateDefault(), _logger);
        }

        /// <summary>
        /// Verifies that <see cref="TokenService.GenerateToken"/> returns a valid JWT containing expected claims.
        /// </summary>
        [Fact]
        public void GenerateToken_ReturnsValidJwt()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();

            var token = _service.GenerateToken(principal);

            Assert.False(string.IsNullOrWhiteSpace(token));
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            Assert.Equal("issuer", jwt.Issuer);
            Assert.Equal("audience", jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "testuser");
            Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
        }

        /// <summary>
        /// Verifies that <see cref="TokenService.GetExpirationTime"/> returns a timestamp in the future within expected bounds.
        /// </summary>
        [Fact]
        public void GetExpirationTime_ReturnsFutureTimestamp()
        {
            var expiration = _service.GetExpirationTime();

            Assert.True(expiration > DateTime.UtcNow);
            Assert.True(expiration <= DateTime.UtcNow.AddMinutes(60));
        }

        /// <summary>
        /// Verifies that <see cref="TokenService.RevokeToken"/> marks a token as revoked and <see cref="TokenService.IsTokenRevoked"/> returns true.
        /// </summary>
        [Fact]
        public void RevokeToken_MarksTokenAsRevoked()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();
            var token = _service.GenerateToken(principal);

            _service.RevokeToken(token);

            Assert.True(_service.IsTokenRevoked(token));
        }

        /// <summary>
        /// Verifies that <see cref="TokenService.IsTokenRevoked"/> returns false for a token that has not been revoked.
        /// </summary>
        [Fact]
        public void IsTokenRevoked_ReturnsFalse_ForUnrevokedToken()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();
            var token = _service.GenerateToken(principal);

            var isRevoked = _service.IsTokenRevoked(token);

            Assert.False(isRevoked);
        }

        /// <summary>
        /// Verifies that <see cref="TokenService.IsTokenRevoked"/> returns false for a malformed token and does not throw.
        /// </summary>
        [Fact]
        public void IsTokenRevoked_ReturnsFalse_ForMalformedToken()
        {
            var malformedToken = "not.a.valid.jwt";

            var isRevoked = _service.IsTokenRevoked(malformedToken);

            Assert.False(isRevoked);
        }
    }
}