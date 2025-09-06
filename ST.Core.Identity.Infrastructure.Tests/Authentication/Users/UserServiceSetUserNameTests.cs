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
        public async Task SetUserNameAsync_ReturnsSuccess_WhenUserNameSet()
        {
            var user = new TestUser { UserName = "testuser" };
            var newUserName = "newuser";
            var userManager = SetUserNameAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetUserNameAsync(user, newUserName);

            Assert.True(result.Succeeded);
            userManager.Verify(um => um.SetUserNameAsync(user, newUserName), Times.Once);
        }

        [Fact]
        public async Task SetUserNameAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            var user = new TestUser { UserName = "testuser" };
            var newUserName = "newuser";
            var error = new IdentityError { Description = "Set username failed." };
            var userManager = SetUserNameAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetUserNameAsync(user, newUserName);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Set username failed.");
            userManager.Verify(um => um.SetUserNameAsync(user, newUserName), Times.Once);
        }

        [Fact]
        public async Task SetUserNameAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var newUserName = "newuser";
            var userManager = SetUserNameAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetUserNameAsync(user, newUserName);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.SetUserNameAsync(user, newUserName), Times.Once);
        }

        [Theory]
        [InlineData(null, "newuser", typeof(ArgumentNullException))]
        [InlineData("user", null, typeof(ArgumentNullException))]
        [InlineData("user", "", typeof(ArgumentException))]
        public async Task SetUserNameAsync_ThrowsForInvalidArguments(string? userType, string? newUserName, Type expectedException)
        {
            var userManager = SetUserNameAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            await Assert.ThrowsAsync(expectedException, () => service.SetUserNameAsync(user!, newUserName!));
        }
    }
}