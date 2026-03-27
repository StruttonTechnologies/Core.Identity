using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.JwtTokenManager;
using StruttonTechnologies.Core.Identity.Stub.Factories;

namespace StruttonTechnologies.Core.Identity.Infrastructure.Tests.TokenManager
{
    [ExcludeFromCodeCoverage]
    public class JwtUserTokenManagerTests
    {
        private static readonly string[] Roles = ["User"];

        private readonly JwtUserTokenManager<Guid> _manager;
        private readonly Mock<IRefreshTokenStore<Guid>> _refreshTokenStore = new();
        private readonly Mock<IAccessTokenRevocationStore<Guid>> _accessTokenRevocationStore = new();

        public JwtUserTokenManagerTests()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();

            _manager = new JwtUserTokenManager<Guid>(
                _refreshTokenStore.Object,
                _accessTokenRevocationStore.Object,
                options);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ReturnsValidJwt()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            JwtSecurityTokenHandler handler = new();
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
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            ClaimsPrincipal? principal = await _manager.ValidateTokenAsync(token);
            Assert.NotNull(principal);
            Assert.Equal("StubUser", principal.FindFirst(ClaimTypes.Name)?.Value);
        }

        [Fact]
        public async Task GetExpirationAsync_ReturnsValidTo()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            DateTime? expiration = await _manager.GetExpirationAsync(token);
            Assert.True(expiration > DateTime.UtcNow);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_MarksTokenAsRevoked()
        {
            Guid userId = Guid.NewGuid();

            string token = await _manager.GenerateAccessTokenAsync(
                userId,
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            _accessTokenRevocationStore
                .Setup(store => store.IsRevokedAsync(It.IsAny<string>(), TestContext.Current.CancellationToken))
                .ReturnsAsync(true);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    TestContext.Current.CancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task IsAccessTokenRevokedAsync_ReturnsFalse_ForUnrevokedToken()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            _accessTokenRevocationStore
                .Setup(store => store.IsRevokedAsync(It.IsAny<string>(), TestContext.Current.CancellationToken))
                .ReturnsAsync(false);

            bool isRevoked = await _manager.IsAccessTokenRevokedAsync(token, TestContext.Current.CancellationToken);

            Assert.False(isRevoked);
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_SavesTokenToStore()
        {
            string token = await _manager.GenerateRefreshTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                TestContext.Current.CancellationToken);

            Assert.False(string.IsNullOrWhiteSpace(token));

            _refreshTokenStore.Verify(
                store => store.SaveAsync(
                    It.Is<RefreshToken<Guid>>(rt => rt.Token == token && rt.Username == "StubUser"),
                    TestContext.Current.CancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_MarksTokenAsRevoked()
        {
            string token = await _manager.GenerateRefreshTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                TestContext.Current.CancellationToken);

            await _manager.RevokeRefreshTokenAsync(token, TestContext.Current.CancellationToken);

            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, TestContext.Current.CancellationToken);
            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsRefreshTokenRevokedAsync_ReturnsFalse_ForUnrevokedToken()
        {
            string token = await _manager.GenerateRefreshTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                TestContext.Current.CancellationToken);

            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, TestContext.Current.CancellationToken);
            Assert.False(isRevoked);
        }
    }
}
