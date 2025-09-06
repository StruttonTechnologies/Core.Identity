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
        public async Task GetLockoutEndDateAsync_ReturnsDate_WhenSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var expectedDate = DateTimeOffset.UtcNow.AddDays(1);
            var userManager = GetLockoutEndDateAsyncMock.WithDate(expectedDate); // For success
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetLockoutEndDateAsync(user);

            Assert.Equal(expectedDate, result);
            userManager.Verify(um => um.GetLockoutEndDateAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetLockoutEndDateAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GetLockoutEndDateAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetLockoutEndDateAsync(user);

            Assert.Null(result);
            userManager.Verify(um => um.GetLockoutEndDateAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetLockoutEndDateAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var userManager = GetLockoutEndDateAsyncMock.WithDate(DateTimeOffset.UtcNow); // For argument validation
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetLockoutEndDateAsync(null!));
        }
    }
}