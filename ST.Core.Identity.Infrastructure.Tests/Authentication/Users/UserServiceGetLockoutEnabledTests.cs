using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task GetLockoutEnabledAsync_ReturnsTrue_WhenLockoutIsEnabled()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GetLockoutEnabledAsyncMock.WithValue(true);  // For enabled
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetLockoutEnabledAsync(user);

            Assert.True(result);
            userManager.Verify(um => um.GetLockoutEnabledAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetLockoutEnabledAsync_ReturnsFalse_WhenLockoutIsDisabled()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GetLockoutEnabledAsyncMock.WithValue(false); // For disabled
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetLockoutEnabledAsync(user);

            Assert.False(result);
            userManager.Verify(um => um.GetLockoutEnabledAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetLockoutEnabledAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GetLockoutEnabledAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetLockoutEnabledAsync(user);

            Assert.False(result);
            userManager.Verify(um => um.GetLockoutEnabledAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetLockoutEnabledAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var userManager = GetLockoutEnabledAsyncMock.WithEnabled();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetLockoutEnabledAsync(null!));
        }
    }
}