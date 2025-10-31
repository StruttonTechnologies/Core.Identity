using Moq;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Infrastructure;
using ST.Core.Identity.JwtTokenManager;
using ST.Core.Identity.Stub.Factories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.TokenManager
{
    public class JwtUserTokenManagerTests
    {
        private readonly IJwtUserTokenManager<Guid> _manager;
        private readonly Mock<IRefreshTokenStore<Guid>> _refreshTokenStore = new();

        public JwtUserTokenManagerTests()
        {
            var options = StubJwtOptionsFactory.CreateDefault(); // returns JwtTokenOptions
            _manager = new JwtUserTokenManager<Guid>(_refreshTokenStore.Object, options);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ReturnsValidJwt()
        {
            var token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(token));

            var jwt = handler.ReadJwtToken(token);
            Assert.Equal("issuer", jwt.Issuer);
            Assert.Equal("audience", jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "StubUser");
            Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
        }

        [Fact]
        public async Task ValidateTokenAsync_ReturnsPrincipal_WhenValid()
        {
            var token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            var principal = await _manager.ValidateTokenAsync(token);
            Assert.NotNull(principal);
            Assert.Equal("StubUser", principal.FindFirst(ClaimTypes.Name)?.Value);
        }

        [Fact]
        public async Task GetExpirationAsync_ReturnsValidTo()
        {
            var token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            var expiration = await _manager.GetExpirationAsync(token);
            Assert.True(expiration > DateTime.UtcNow);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_MarksTokenAsRevoked()
        {
            var token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            await _manager.RevokeAccessTokenAsync(token, default);
            var isRevoked = await _manager.IsAccessTokenRevokedAsync(token, default);

            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsAccessTokenRevokedAsync_ReturnsFalse_ForUnrevokedToken()
        {
            var token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(), "StubUser", "stub@example.com", new[] { "User" }, default);

            var isRevoked = await _manager.IsAccessTokenRevokedAsync(token, default);
            Assert.False(isRevoked);
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_SavesTokenToStore()
        {
            var token = await _manager.GenerateRefreshTokenAsync(Guid.NewGuid(), "StubUser", default);
            Assert.False(string.IsNullOrWhiteSpace(token));

            _refreshTokenStore.Verify(store => store.SaveAsync(
                It.Is<RefreshToken<Guid>>(rt => rt.Token == token && rt.Username == "StubUser"),
                default), Times.Once);
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_MarksTokenAsRevoked()
        {
            var token = await _manager.GenerateRefreshTokenAsync(Guid.NewGuid(), "StubUser", default);
            await _manager.RevokeRefreshTokenAsync(token, default);

            var isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, default);
            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsRefreshTokenRevokedAsync_ReturnsFalse_ForUnrevokedToken()
        {
            var token = await _manager.GenerateRefreshTokenAsync(Guid.NewGuid(), "StubUser", default);
            var isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, default);
            Assert.False(isRevoked);
        }
    }
}