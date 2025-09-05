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
        public async Task SetEmailAsync_ReturnsSuccess_WhenEmailSet()
        {
            var user = new TestUser { UserName = "testuser" };
            var email = "user@example.com";
            var userManager = SetEmailAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetEmailAsync(user, email);

            Assert.True(result.Succeeded);
            userManager.Verify(um => um.SetEmailAsync(user, email), Times.Once);
        }

        [Fact]
        public async Task SetEmailAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            var user = new TestUser { UserName = "testuser" };
            var email = "user@example.com";
            var error = new IdentityError { Description = "Set email failed." };
            var userManager = SetEmailAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetEmailAsync(user, email);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Set email failed.");
            userManager.Verify(um => um.SetEmailAsync(user, email), Times.Once);
        }

        [Fact]
        public async Task SetEmailAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var newEmail = "new@example.com";
            var userManager = SetEmailAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetEmailAsync(user, newEmail);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.SetEmailAsync(user, newEmail), Times.Once);
        }

        [Theory]
        [InlineData(null, "user@example.com", typeof(ArgumentNullException))]
        [InlineData("user", null, typeof(ArgumentException))]
        [InlineData("user", "", typeof(ArgumentException))]
        public async Task SetEmailAsync_ThrowsForInvalidArguments(string? userType, string? email, Type expectedException)
        {
            var userManager = SetEmailAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            await Assert.ThrowsAsync(expectedException, () => service.SetEmailAsync(user!, email!));
        }
    }
}