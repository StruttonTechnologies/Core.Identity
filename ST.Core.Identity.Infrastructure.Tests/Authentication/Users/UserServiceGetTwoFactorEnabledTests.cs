using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.TwoFactor;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task GetTwoFactorEnabledAsync_ReturnsTrue_WhenTwoFactorIsEnabled()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = GetTwoFactorEnabledAsyncMock.WithValue(true);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetTwoFactorEnabledAsync(user, cancellationToken);

            Assert.True(result);
            userManager.Verify(um => um.GetTwoFactorEnabledAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetTwoFactorEnabledAsync_ReturnsFalse_WhenTwoFactorIsDisabled()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = GetTwoFactorEnabledAsyncMock.WithValue(false);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetTwoFactorEnabledAsync(user, cancellationToken);

            Assert.False(result);
            userManager.Verify(um => um.GetTwoFactorEnabledAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetTwoFactorEnabledAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = GetTwoFactorEnabledAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetTwoFactorEnabledAsync(user, cancellationToken);

            Assert.False(result);
            userManager.Verify(um => um.GetTwoFactorEnabledAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetTwoFactorEnabledAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var cancellationToken = CancellationToken.None;
            var userManager = GetTwoFactorEnabledAsyncMock.WithValue(true);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetTwoFactorEnabledAsync(null!, cancellationToken));
        }
    }
}