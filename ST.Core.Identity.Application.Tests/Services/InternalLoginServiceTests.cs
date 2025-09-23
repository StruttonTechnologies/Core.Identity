using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Application.Authentication.Services.Authentication;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Testable;
using ST.Core.IdentityAccess.Fakes.JwtToken;
using ST.Core.IdentityAccess.Fakes.Stores;
using ST.Core.IdentityAccess.Fakes.UserManager;
using System.Security;

namespace ST.Core.Identity.Application.Tests.Services
{
    public class InternalLoginServiceTests
    {
        private readonly TestUserManager _userManager;
        private readonly Mock<IJwtUserTokenManager<Guid>> _tokenManagerMock;
        private readonly ILogger<InternalLoginService<TestAppIdentityUser, Guid>> _logger;

        private readonly InternalLoginService<TestAppIdentityUser, Guid> _service;

        public InternalLoginServiceTests()
        {
            var store = new InMemoryUserStore();
            _userManager = new TestUserManager(store);

            _tokenManagerMock = new Mock<IJwtUserTokenManager<Guid>>();
            _logger = MockLoggerFactory.Create<InternalLoginService<TestAppIdentityUser, Guid>>();

            _service = new InternalLoginService<TestAppIdentityUser, Guid>(
                _userManager,
                _tokenManagerMock.Object,
                _logger);
        }

        [Fact]
        public async Task EnsureUserNotLockedOutAsync_LockedOutUser_ThrowsSecurityException()
        {
            var user = TestAppUserIdentityFactory.LockedOut();
            var service = new TestableInternalLoginService(
                new TestUserManager(new InMemoryUserStore()),
                new TestTokenManager(),
                MockLoggerFactory.Create<InternalLoginService<TestAppIdentityUser, Guid>>());

            await Assert.ThrowsAsync<SecurityException>(() =>
                service.ExposeEnsureUserNotLockedOutAsync(user));
        }

        [Fact]
        public async Task FindUserRecordAsync_WithValidUsername_ReturnsUser()
        {
            var user = TestAppUserIdentityFactory.Create("valid.user");
            await _userManager.CreateAsync(user);

            var service = new TestableInternalLoginService(_userManager, new TestTokenManager(), _logger);

            var result = await service.ExposeFindUserRecordAsync(user.UserName, CancellationToken.None);

            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async Task FindUserRecordAsync_WithUnknownUser_ThrowsException()
        {
            var service = new TestableInternalLoginService(_userManager, new TestTokenManager(), _logger);

            await Assert.ThrowsAsync<Exception>(() =>
                service.ExposeFindUserRecordAsync("unknown.user", CancellationToken.None));
        }

        [Fact]
        public async Task EnsureValidPasswordAsync_WithCorrectPassword_DoesNotThrow()
        {
            var user = TestAppUserIdentityFactory.Create("secure.user");
            await _userManager.CreateAsync(user);
            await _userManager.AddPasswordAsync(user, "Secure123!");

            var service = new TestableInternalLoginService(_userManager, new TestTokenManager(), _logger);

            await service.ExposeEnsureValidPasswordAsync(user, "Secure123!");
        }

        [Fact]
        public async Task EnsureValidPasswordAsync_WithIncorrectPassword_ThrowsUnauthorizedAccessException()
        {
            var user = TestAppUserIdentityFactory.Create("secure.user");
            await _userManager.CreateAsync(user);
            await _userManager.AddPasswordAsync(user, "Secure123!");

            var service = new TestableInternalLoginService(_userManager, new TestTokenManager(), _logger);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                service.ExposeEnsureValidPasswordAsync(user, "WrongPassword"));
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ReturnsExpectedFormat()
        {
            var user = TestAppUserIdentityFactory.Create("token.user");
            var roles = new List<string> { "Admin", "User" };

            var service = new TestableInternalLoginService(_userManager, new TestTokenManager(), _logger);

            var token = await service.ExposeGenerateAccessTokenAsync(user, roles, CancellationToken.None);

            Assert.StartsWith("access-token-for-token.user", token);
        }

        [Fact]
        public async Task BuildLoginResponseAsync_ReturnsExpectedDto()
        {
            var user = TestAppUserIdentityFactory.Create("response.user");
            var accessToken = "access-token";
            var refreshToken = "refresh-token";

            var service = new TestableInternalLoginService(_userManager, new TestTokenManager(), _logger);

            var response = await service.ExposeBuildLoginResponseAsync(user, accessToken, refreshToken, CancellationToken.None);

            Assert.Equal(accessToken, response.AccessToken);
            Assert.Equal(refreshToken, response.RefreshToken);
            Assert.Equal(user.UserName, response.Username);
            Assert.False(response.RequiresTwoFactor);
        }
    }
}
