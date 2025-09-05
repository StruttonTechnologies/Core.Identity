using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task FindByIdAsync_ReturnsUser_WhenUserFound()
        {
            var userId = "user-id";
            var user = new TestUser { Id = userId };
            var userManager = FindByIdAsyncMock.WithUser(user);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByIdAsync(userId);

            Assert.Equal(user, result);
            userManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull_WhenUserNotFound()
        {
            var userId = "notfound-id";
            var userManager = FindByIdAsyncMock.WithNull();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByIdAsync(userId);

            Assert.Null(result);
            userManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var userId = "error-id";
            var userManager = FindByIdAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByIdAsync(userId);

            Assert.Null(result);
            userManager.Verify(um => um.FindByIdAsync(userId), Times.Once);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task FindByIdAsync_ThrowsForInvalidArguments(string? userId, Type expectedException)
        {
            var userManager = FindByIdAsyncMock.WithNull();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync(expectedException, () => service.FindByIdAsync(userId!));
        }
    }
}