using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Options;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
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
    [ExcludeFromCodeCoverage]
    public class TokenOrchestrationTests
    {
        private readonly StubRevocableTokenManager _jwtManager;
        private readonly TokenOrchestration<Guid> _service;
        private readonly Mock<IJwtUserTokenManager<Guid>> _mockJwtManager;
        private readonly TokenOrchestration<Guid> _mockService;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenOrchestrationTests"/> using default token options and stubbed token manager.
        /// </summary>
        public TokenOrchestrationTests()
        {
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            _jwtManager = new StubRevocableTokenManager();
            _service = new TokenOrchestration<Guid>(options, _jwtManager);

            _mockJwtManager = new Mock<IJwtUserTokenManager<Guid>>();
            _mockService = new TokenOrchestration<Guid>(options, _mockJwtManager.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenOptionsIsNull()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
                new TokenOrchestration<Guid>(null!, _jwtManager));

            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenJwtManagerIsNull()
        {
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());

            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
                new TokenOrchestration<Guid>(options, null!));

            Assert.Equal("jwtManager", ex.ParamName);
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
        public async Task GenerateTokenAsync_ThrowsInvalidOperationException_WhenPrincipalIsNull()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.GenerateTokenAsync(null!, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GenerateTokenAsync_ThrowsInvalidOperationException_WhenIdentityIsNotClaimsIdentity()
        {
            ClaimsPrincipal principal = new(new ClaimsIdentity());

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GenerateTokenAsync_ThrowsInvalidOperationException_WhenUserIdIsMissing()
        {
            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GenerateTokenAsync_WorksWithIntKey()
        {
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            Mock<IJwtUserTokenManager<int>> jwtManager = new();
            TokenOrchestration<int> service = new(options, jwtManager.Object);

            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Email, "test@example.com")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            jwtManager
                .Setup(m => m.GenerateAccessTokenAsync(123, "TestUser", "test@example.com", It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("test-token");

            string token = await service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken);

            Assert.Equal("test-token", token);
        }

        [Fact]
        public async Task GenerateTokenAsync_WorksWithLongKey()
        {
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            Mock<IJwtUserTokenManager<long>> jwtManager = new();
            TokenOrchestration<long> service = new(options, jwtManager.Object);

            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "999999999999"),
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Email, "test@example.com")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            jwtManager
                .Setup(m => m.GenerateAccessTokenAsync(999999999999L, "TestUser", "test@example.com", It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("test-token");

            string token = await service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken);

            Assert.Equal("test-token", token);
        }

        [Fact]
        public async Task GenerateTokenAsync_ThrowsInvalidCastException_ForInvalidGuid()
        {
            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "not-a-guid"),
                new Claim(ClaimTypes.Name, "TestUser")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            await Assert.ThrowsAsync<InvalidCastException>(async () =>
                await _service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GenerateTokenAsync_ThrowsInvalidCastException_ForInvalidInt()
        {
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            Mock<IJwtUserTokenManager<int>> jwtManager = new();
            TokenOrchestration<int> service = new(options, jwtManager.Object);

            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "not-a-number"),
                new Claim(ClaimTypes.Name, "TestUser")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            await Assert.ThrowsAsync<InvalidCastException>(async () =>
                await service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GenerateTokenAsync_ThrowsInvalidCastException_ForInvalidLong()
        {
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            Mock<IJwtUserTokenManager<long>> jwtManager = new();
            TokenOrchestration<long> service = new(options, jwtManager.Object);

            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "not-a-number"),
                new Claim(ClaimTypes.Name, "TestUser")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            await Assert.ThrowsAsync<InvalidCastException>(async () =>
                await service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GenerateTokenAsync_UsesConvertChangeType_ForOtherTypes()
        {
            IOptions<JwtTokenOptions> options = Options.Create(StubJwtOptionsFactory.CreateDefault());
            Mock<IJwtUserTokenManager<string>> jwtManager = new();
            TokenOrchestration<string> service = new(options, jwtManager.Object);

            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user123"),
                new Claim(ClaimTypes.Name, "TestUser")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            jwtManager
                .Setup(m => m.GenerateAccessTokenAsync("user123", "TestUser", "", It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("test-token");

            string token = await service.GenerateTokenAsync(principal, TestContext.Current.CancellationToken);

            Assert.Equal("test-token", token);
        }

        [Fact]
        public void GetExpirationTime_ReturnsFutureTimestamp()
        {
            DateTime expiration = _service.GetExpirationTime();

            Assert.True(expiration > DateTime.UtcNow);
            Assert.True(expiration <= DateTime.UtcNow.AddMinutes(60));
        }

        [Fact]
        public async Task RevokeAccessTokenAsync_CallsJwtManager()
        {
            string token = "test-token";

            _mockJwtManager
                .Setup(m => m.RevokeAccessTokenAsync(token, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mockService.RevokeAccessTokenAsync(token, TestContext.Current.CancellationToken);

            _mockJwtManager.Verify(m => m.RevokeAccessTokenAsync(token, It.IsAny<CancellationToken>()), Times.Once);
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
        public async Task IsAccessTokenRevokedAsync_CallsJwtManager()
        {
            string token = "test-token";

            _mockJwtManager
                .Setup(m => m.IsAccessTokenRevokedAsync(token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            bool result = await _mockService.IsAccessTokenRevokedAsync(token, TestContext.Current.CancellationToken);

            Assert.True(result);
            _mockJwtManager.Verify(m => m.IsAccessTokenRevokedAsync(token, It.IsAny<CancellationToken>()), Times.Once);
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

        [Fact]
        public async Task RevokeRefreshTokenAsync_CallsJwtManager()
        {
            string token = "refresh-token";

            _mockJwtManager
                .Setup(m => m.RevokeRefreshTokenAsync(token, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mockService.RevokeRefreshTokenAsync(token, TestContext.Current.CancellationToken);

            _mockJwtManager.Verify(m => m.RevokeRefreshTokenAsync(token, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task IsRefreshTokenRevokedAsync_CallsJwtManager()
        {
            string token = "refresh-token";

            _mockJwtManager
                .Setup(m => m.IsRefreshTokenRevokedAsync(token, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            bool result = await _mockService.IsRefreshTokenRevokedAsync(token, TestContext.Current.CancellationToken);

            Assert.False(result);
            _mockJwtManager.Verify(m => m.IsRefreshTokenRevokedAsync(token, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ValidateTokenAsync_CallsJwtManager()
        {
            string token = "valid-token";
            ClaimsPrincipal expectedPrincipal = TestClaimsPrincipalBuilder.CreateDefault();

            _mockJwtManager
                .Setup(m => m.ValidateTokenAsync(token))
                .ReturnsAsync(expectedPrincipal);

            ClaimsPrincipal? result = await _mockService.ValidateTokenAsync(token, TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.Equal(expectedPrincipal, result);
            _mockJwtManager.Verify(m => m.ValidateTokenAsync(token), Times.Once);
        }

        [Fact]
        public async Task GetExpirationAsync_CallsJwtManager()
        {
            string token = "test-token";
            DateTime expectedExpiration = DateTime.UtcNow.AddMinutes(30);

            _mockJwtManager
                .Setup(m => m.GetExpirationAsync(token))
                .ReturnsAsync(expectedExpiration);

            DateTime? result = await _mockService.GetExpirationAsync(token, TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            Assert.Equal(expectedExpiration, result.Value);
            _mockJwtManager.Verify(m => m.GetExpirationAsync(token), Times.Once);
        }
    }
}
