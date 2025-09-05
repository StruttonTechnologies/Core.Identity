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
        public async Task FindByNameAsync_ReturnsUser_WhenUserFound()
        {
            var username = "testuser";
            var user = new TestUser { UserName = username };
            var userManager = FindByNameAsyncMock.WithUser(user);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByNameAsync(username);

            Assert.Equal(user, result);
            userManager.Verify(um => um.FindByNameAsync(username), Times.Once);
        }

        [Fact]
        public async Task FindByNameAsync_ReturnsNull_WhenUserNotFound()
        {
            var username = "notfound";
            var userManager = FindByNameAsyncMock.WithNull();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByNameAsync(username);

            Assert.Null(result);
            userManager.Verify(um => um.FindByNameAsync(username), Times.Once);
        }

        [Fact]
        public async Task FindByNameAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var username = "erroruser";
            var userManager = FindByNameAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByNameAsync(username);

            Assert.Null(result);
            userManager.Verify(um => um.FindByNameAsync(username), Times.Once);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task FindByNameAsync_ThrowsForInvalidArguments(string? username, Type expectedException)
        {
            var userManager = FindByNameAsyncMock.WithNull();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync(expectedException, () => service.FindByNameAsync(username!));
        }
    }
}