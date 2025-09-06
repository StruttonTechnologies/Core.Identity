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
        public async Task ResetAccessFailedCountAsync_ReturnsSuccess_WhenResetSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = ResetAccessFailedCountAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.ResetAccessFailedCountAsync(user);

            Assert.True(result.Succeeded);
            userManager.Verify(um => um.ResetAccessFailedCountAsync(user), Times.Once);
        }

        [Fact]
        public async Task ResetAccessFailedCountAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            var user = new TestUser { UserName = "testuser" };
            var error = new IdentityError { Description = "Reset failed." };
            var userManager = ResetAccessFailedCountAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.ResetAccessFailedCountAsync(user);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Reset failed.");
            userManager.Verify(um => um.ResetAccessFailedCountAsync(user), Times.Once);
        }

        [Fact]
        public async Task ResetAccessFailedCountAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = ResetAccessFailedCountAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.ResetAccessFailedCountAsync(user);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.ResetAccessFailedCountAsync(user), Times.Once);
        }

        [Fact]
        public async Task ResetAccessFailedCountAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var userManager = ResetAccessFailedCountAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ResetAccessFailedCountAsync(null!));
        }
    }
}