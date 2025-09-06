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
        public async Task GetAccessFailedCountAsync_ReturnsCount_WhenSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var expectedCount = 3;
            var userManager = GetAccessFailedCountAsyncMock.WithCount(expectedCount);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetAccessFailedCountAsync(user);

            Assert.Equal(expectedCount, result);
            userManager.Verify(um => um.GetAccessFailedCountAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetAccessFailedCountAsync_ReturnsZero_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GetAccessFailedCountAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetAccessFailedCountAsync(user);

            Assert.Equal(0, result);
            userManager.Verify(um => um.GetAccessFailedCountAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetAccessFailedCountAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var userManager = GetAccessFailedCountAsyncMock.WithCount(1);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetAccessFailedCountAsync(null!));
        }
    }
}