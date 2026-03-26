using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;
using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Domain.Interfaces.Jwtoken;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.JwtTokenManager;
using StruttonTechnologies.Core.Identity.Stub.Factories;

namespace StruttonTechnologies.Core.Identity.Infrastructure.Tests.TokenManager
{
    public class JwtUserTokenManagerTests
    {
        private readonly IJwtUserTokenManager<Guid> _manager;
        private readonly Mock<IRefreshTokenStore<Guid>> _refreshTokenStore = new();
        private static readonly string[] roles = new[] { "User" };

        public JwtUserTokenManagerTests()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault(); // returns JwtTokenOptions
            _manager = new JwtUserTokenManager<Guid>(_refreshTokenStore.Object, options);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ReturnsValidJwt()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", roles, default);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(token));

            JwtSecurityToken jwt = handler.ReadJwtToken(token);
            Assert.Equal("issuer", jwt.Issuer);
            Assert.Equal("audience", jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "StubUser");
            Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
        }

        [Fact]
        public async Task ValidateTokenAsync_ReturnsPrincipal_WhenValid()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", roles, default);

            ClaimsPrincipal? principal = await _manager.ValidateTokenAsync(token);
            Assert.NotNull(principal);
            Assert.Equal("StubUser", principal.FindFirst(ClaimTypes.Name)?.Value);
        }

        [Fact]
        public async Task GetExpirationAsync_ReturnsValidTo()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            DateTime? expiration = await _manager.GetExpirationAsync(token);
            Assert.True(expiration > DateTime.UtcNow);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_MarksTokenAsRevoked()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            await _manager.RevokeAccessTokenAsync(token, default);
            bool isRevoked = await _manager.IsAccessTokenRevokedAsync(token, default);

            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsAccessTokenRevokedAsync_ReturnsFalse_ForUnrevokedToken()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            bool isRevoked = await _manager.IsAccessTokenRevokedAsync(token, default);
            Assert.False(isRevoked);
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_SavesTokenToStore()
        {
            string token = await _manager.GenerateRefreshTokenAsync(Guid.NewGuid(), "StubUser", default);
            Assert.False(string.IsNullOrWhiteSpace(token));

            _refreshTokenStore.Verify(store => store.SaveAsync(
                It.Is<RefreshToken<Guid>>(rt => rt.Token == token && rt.Username == "StubUser"),
                default), Times.Once);
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_MarksTokenAsRevoked()
        {
            string token = await _manager.GenerateRefreshTokenAsync(Guid.NewGuid(), "StubUser", default);
            await _manager.RevokeRefreshTokenAsync(token, default);

            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, default);
            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsRefreshTokenRevokedAsync_ReturnsFalse_ForUnrevokedToken()
        {
            string token = await _manager.GenerateRefreshTokenAsync(Guid.NewGuid(), "StubUser", default);
            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, default);
            Assert.False(isRevoked);
        }
    }
}
