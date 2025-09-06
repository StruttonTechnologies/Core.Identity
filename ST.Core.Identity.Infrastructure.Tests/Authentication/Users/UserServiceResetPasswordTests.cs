using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Password;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task ResetPasswordAsync_ReturnsSuccess_WhenResetSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var token = "token";
            var newPassword = "newPassword";
            var userManager = ResetPasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.ResetPasswordAsync(user, token, newPassword);

            Assert.True(result.Succeeded);
            userManager.Verify(um => um.ResetPasswordAsync(user, token, newPassword), Times.Once);
        }

        [Fact]
        public async Task ResetPasswordAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            var user = new TestUser { UserName = "testuser" };
            var token = "token";
            var newPassword = "newPassword";
            var error = new IdentityError { Description = "Reset failed." };
            var userManager = ResetPasswordAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.ResetPasswordAsync(user, token, newPassword);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Reset failed.");
            userManager.Verify(um => um.ResetPasswordAsync(user, token, newPassword), Times.Once);
        }

        [Fact]
        public async Task ResetPasswordAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var token = "reset-token";
            var newPassword = "newPassword";
            var userManager = ResetPasswordAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.ResetPasswordAsync(user, token, newPassword);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.ResetPasswordAsync(user, token, newPassword), Times.Once);
        }

        [Theory]
        [InlineData(null, "token", "newPassword", typeof(ArgumentNullException))]
        [InlineData("user", null, "newPassword", typeof(ArgumentException))]
        [InlineData("user", "", "newPassword", typeof(ArgumentException))]
        [InlineData("user", "token", null, typeof(ArgumentException))]
        [InlineData("user", "token", "", typeof(ArgumentException))]
        public async Task ResetPasswordAsync_ThrowsForInvalidArguments(string? userType, string? token, string? newPassword, Type expectedException)
        {
            var userManager = ResetPasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            await Assert.ThrowsAsync(expectedException, () => service.ResetPasswordAsync(user!, token!, newPassword!));
        }
    }
}