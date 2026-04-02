using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.JwtTokenManager;
using StruttonTechnologies.Core.Identity.Stub.Factories;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);
            string? jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            Assert.False(string.IsNullOrWhiteSpace(jti), "Token should have a JTI claim");

            _accessTokenRevocationStore
                .Setup(store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
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
            Guid userId = Guid.NewGuid();
            string token = await _manager.GenerateRefreshTokenAsync(
                userId,
                "StubUser",
                TestContext.Current.CancellationToken);

            _refreshTokenStore
                .Setup(store => store.GetAsync(token, TestContext.Current.CancellationToken))
                .ReturnsAsync(new RefreshToken<Guid>
                {
                    Token = token,
                    UserId = userId,
                    Username = "StubUser",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(30),
                    IsRevoked = false
                });

            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, TestContext.Current.CancellationToken);
            Assert.False(isRevoked);
        }

        [Fact]
        public async Task ValidateTokenAsync_ReturnsNull_ForInvalidToken()
        {
            ClaimsPrincipal? principal = await _manager.ValidateTokenAsync("invalid-token");

            Assert.Null(principal);
        }

        [Fact]
        public async Task ValidateTokenAsync_ReturnsNull_WhenTokenIsRevoked()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            _accessTokenRevocationStore
                .Setup(store => store.IsRevokedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            ClaimsPrincipal? principal = await _manager.ValidateTokenAsync(token);

            Assert.Null(principal);
        }

        [Fact]
        public async Task IsAccessTokenRevokedAsync_ReturnsTrue_ForRevokedToken()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            _accessTokenRevocationStore
                .Setup(store => store.IsRevokedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            bool isRevoked = await _manager.IsAccessTokenRevokedAsync(token, TestContext.Current.CancellationToken);

            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsRefreshTokenRevokedAsync_ReturnsTrue_ForRevokedToken()
        {
            Guid userId = Guid.NewGuid();
            string token = await _manager.GenerateRefreshTokenAsync(
                userId,
                "StubUser",
                TestContext.Current.CancellationToken);

            _refreshTokenStore
                .Setup(store => store.GetAsync(token, TestContext.Current.CancellationToken))
                .ReturnsAsync(new RefreshToken<Guid>
                {
                    Token = token,
                    UserId = userId,
                    Username = "StubUser",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(30),
                    IsRevoked = true
                });

            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, TestContext.Current.CancellationToken);

            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsRefreshTokenRevokedAsync_ReturnsTrue_ForExpiredToken()
        {
            Guid userId = Guid.NewGuid();
            string token = await _manager.GenerateRefreshTokenAsync(
                userId,
                "StubUser",
                TestContext.Current.CancellationToken);

            _refreshTokenStore
                .Setup(store => store.GetAsync(token, TestContext.Current.CancellationToken))
                .ReturnsAsync(new RefreshToken<Guid>
                {
                    Token = token,
                    UserId = userId,
                    Username = "StubUser",
                    CreatedAt = DateTime.UtcNow.AddDays(-31),
                    ExpiresAt = DateTime.UtcNow.AddDays(-1),
                    IsRevoked = false
                });

            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync(token, TestContext.Current.CancellationToken);

            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsRefreshTokenRevokedAsync_ReturnsTrue_WhenTokenNotFound()
        {
            _refreshTokenStore
                .Setup(store => store.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken<Guid>?)null);

            bool isRevoked = await _manager.IsRefreshTokenRevokedAsync("non-existent-token", TestContext.Current.CancellationToken);

            Assert.True(isRevoked);
        }

        [Fact]
        public async Task RevokeAccessTokensAsync_CallsStoreRevokeAllAsync()
        {
            Guid userId = Guid.NewGuid();

            _accessTokenRevocationStore
                .Setup(store => store.RevokeAllAsync(userId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _manager.RevokeAccessTokensAsync(userId, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAllAsync(userId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RevokeRefreshTokensAsync_CallsStoreRevokeAllAsync()
        {
            Guid userId = Guid.NewGuid();

            _refreshTokenStore
                .Setup(store => store.RevokeAllAsync(userId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _manager.RevokeRefreshTokensAsync(userId, TestContext.Current.CancellationToken);

            _refreshTokenStore.Verify(
                store => store.RevokeAllAsync(userId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenRefreshTokenStoreIsNull()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();

            Assert.Throws<ArgumentNullException>(() =>
                new JwtUserTokenManager<Guid>(
                    null!,
                    _accessTokenRevocationStore.Object,
                    options));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenAccessTokenRevocationStoreIsNull()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();

            Assert.Throws<ArgumentNullException>(() =>
                new JwtUserTokenManager<Guid>(
                    _refreshTokenStore.Object,
                    null!,
                    options));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new JwtUserTokenManager<Guid>(
                    _refreshTokenStore.Object,
                    _accessTokenRevocationStore.Object,
                    null!));
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ThrowsArgumentNullException_WhenUsernameIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _manager.GenerateAccessTokenAsync(
                    Guid.NewGuid(),
                    null!,
                    "stub@example.com",
                    Roles,
                    TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_HandlesNullEmail()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                null!,
                Roles,
                TestContext.Current.CancellationToken);

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Email && c.Value == string.Empty);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_HandlesNullRoles()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                null!,
                TestContext.Current.CancellationToken);

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            Assert.DoesNotContain(jwt.Claims, c => c.Type == ClaimTypes.Role);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_HandlesEmptyRoles()
        {
            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                Array.Empty<string>(),
                TestContext.Current.CancellationToken);

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            Assert.DoesNotContain(jwt.Claims, c => c.Type == ClaimTypes.Role);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_HandlesMultipleRoles()
        {
            string[] multipleRoles = ["User", "Admin", "Manager"];

            string token = await _manager.GenerateAccessTokenAsync(
                Guid.NewGuid(),
                "StubUser",
                "stub@example.com",
                multipleRoles,
                TestContext.Current.CancellationToken);

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            List<Claim> roleClaims = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            Assert.Equal(3, roleClaims.Count);
            Assert.Contains(roleClaims, c => c.Value == "User");
            Assert.Contains(roleClaims, c => c.Value == "Admin");
            Assert.Contains(roleClaims, c => c.Value == "Manager");
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_IncludesUserIdClaim()
        {
            Guid userId = Guid.NewGuid();

            string token = await _manager.GenerateAccessTokenAsync(
                userId,
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            Claim? nameIdentifierClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Assert.NotNull(nameIdentifierClaim);
            Assert.Equal(userId.ToString(), nameIdentifierClaim.Value);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_DoesNothing_WhenTokenHasNoJtiClaim()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));

            JwtSecurityToken tokenWithoutJti = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithoutJti);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_HandlesTokenWithEmptyJti()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, string.Empty));

            JwtSecurityToken tokenWithEmptyJti = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithEmptyJti);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_WorksWithoutSubjectClaim()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithoutSub = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithoutSub);

            _accessTokenRevocationStore
                .Setup(store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.Is<Guid>(g => g == default),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task IsAccessTokenRevokedAsync_ReturnsFalse_WhenTokenHasNoJti()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));

            JwtSecurityToken tokenWithoutJti = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithoutJti);

            bool isRevoked = await _manager.IsAccessTokenRevokedAsync(token, TestContext.Current.CancellationToken);

            Assert.False(isRevoked);
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_CreatesTokenWithCorrectProperties()
        {
            Guid userId = Guid.NewGuid();
            string username = "TestUser";
            RefreshToken<Guid>? savedToken = null;

            _refreshTokenStore
                .Setup(store => store.SaveAsync(It.IsAny<RefreshToken<Guid>>(), It.IsAny<CancellationToken>()))
                .Callback<RefreshToken<Guid>, CancellationToken>((token, ct) => savedToken = token)
                .Returns(Task.CompletedTask);

            string token = await _manager.GenerateRefreshTokenAsync(userId, username, TestContext.Current.CancellationToken);

            Assert.NotNull(savedToken);
            Assert.Equal(userId, savedToken.UserId);
            Assert.Equal(username, savedToken.Username);
            Assert.False(savedToken.IsRevoked);
            Assert.True(savedToken.ExpiresAt > DateTime.UtcNow);
            Assert.True(savedToken.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public async Task ValidateTokenAsync_ReturnsNull_ForExpiredToken()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken expiredToken = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow.AddMinutes(-60),
                expires: DateTime.UtcNow.AddMinutes(-30),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(expiredToken);

            ClaimsPrincipal? principal = await _manager.ValidateTokenAsync(token);

            Assert.Null(principal);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_HandlesInvalidGuidFormat()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "not-a-valid-guid"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithInvalidGuid = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithInvalidGuid);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                                                  Times.Never);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_UsesConvertChangeType_ForNonGuidKeyType()
        {
            Mock<IRefreshTokenStore<int>> refreshTokenStoreInt = new();
            Mock<IAccessTokenRevocationStore<int>> accessTokenRevocationStoreInt = new();
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();

            JwtUserTokenManager<int> managerInt = new(
                refreshTokenStoreInt.Object,
                accessTokenRevocationStoreInt.Object,
                options);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "123"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithIntUserId = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithIntUserId);

            accessTokenRevocationStoreInt
                .Setup(store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await managerInt.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            accessTokenRevocationStoreInt.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    123,
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_HandlesOverflowException_ForSmallKeyType()
        {
            Mock<IRefreshTokenStore<sbyte>> refreshTokenStoreSByte = new();
            Mock<IAccessTokenRevocationStore<sbyte>> accessTokenRevocationStoreSByte = new();
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();

            JwtUserTokenManager<sbyte> managerSByte = new(
                refreshTokenStoreSByte.Object,
                accessTokenRevocationStoreSByte.Object,
                options);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "999999"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithOverflow = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithOverflow);

            await managerSByte.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            accessTokenRevocationStoreSByte.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<sbyte>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetExpirationAsync_ThrowsSecurityTokenMalformedException_ForMalformedToken()
        {
            await Assert.ThrowsAsync<SecurityTokenMalformedException>(async () =>
                await _manager.GetExpirationAsync("not-a-valid-jwt-token"));
        }

        [Fact]
        public async Task IsAccessTokenRevokedAsync_ThrowsSecurityTokenMalformedException_ForMalformedToken()
        {
            await Assert.ThrowsAsync<SecurityTokenMalformedException>(async () =>
                await _manager.IsAccessTokenRevokedAsync("not-a-valid-jwt-token", TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_HandlesInvalidCastException_ForNonConvertibleType()
        {
            Mock<IRefreshTokenStore<double>> refreshTokenStoreDouble = new();
            Mock<IAccessTokenRevocationStore<double>> accessTokenRevocationStoreDouble = new();
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();

            JwtUserTokenManager<double> managerDouble = new(
                refreshTokenStoreDouble.Object,
                accessTokenRevocationStoreDouble.Object,
                options);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "not-a-number"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithInvalidDouble = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithInvalidDouble);

            await managerDouble.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            accessTokenRevocationStoreDouble.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_HandlesWhitespaceJti()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, "   "));

            JwtSecurityToken tokenWithWhitespaceJti = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithWhitespaceJti);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_HandlesWhitespaceSubjectClaim()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "   "));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithWhitespaceSub = new(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithWhitespaceSub);

            _accessTokenRevocationStore
                .Setup(store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _manager.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _accessTokenRevocationStore.Verify(
                store => store.RevokeAsync(
                    It.IsAny<string>(),
                    It.Is<Guid>(g => g == default),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ValidateTokenAsync_ReturnsNull_ForInvalidIssuer()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithInvalidIssuer = new(
                issuer: "wrong-issuer",
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithInvalidIssuer);

            ClaimsPrincipal? principal = await _manager.ValidateTokenAsync(token);

            Assert.Null(principal);
        }

        [Fact]
        public async Task ValidateTokenAsync_ReturnsNull_ForInvalidAudience()
        {
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity identity = new("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.Name, "StubUser"));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            JwtSecurityToken tokenWithInvalidAudience = new(
                issuer: options.Issuer,
                audience: "wrong-audience",
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenWithInvalidAudience);

            ClaimsPrincipal? principal = await _manager.ValidateTokenAsync(token);

            Assert.Null(principal);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_UsesLongKeyType()
        {
            Mock<IRefreshTokenStore<long>> refreshTokenStoreLong = new();
            Mock<IAccessTokenRevocationStore<long>> accessTokenRevocationStoreLong = new();
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();

            JwtUserTokenManager<long> managerLong = new(
                refreshTokenStoreLong.Object,
                accessTokenRevocationStoreLong.Object,
                options);

            long userId = 123456789L;

            string token = await managerLong.GenerateAccessTokenAsync(
                userId,
                "StubUser",
                "stub@example.com",
                Roles,
                TestContext.Current.CancellationToken);

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            Claim? nameIdentifierClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Assert.NotNull(nameIdentifierClaim);
            Assert.Equal(userId.ToString(), nameIdentifierClaim.Value);
        }
    }
}
