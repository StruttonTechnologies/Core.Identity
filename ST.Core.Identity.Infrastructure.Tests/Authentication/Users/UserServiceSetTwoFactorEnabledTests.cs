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
        public async Task SetTwoFactorEnabledAsync_ReturnsSuccess_WhenSetSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var enabled = true;
            var userManager = SetTwoFactorEnabledAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetTwoFactorEnabledAsync(user, enabled);

            Assert.True(result.Succeeded);
            userManager.Verify(um => um.SetTwoFactorEnabledAsync(user, enabled), Times.Once);
        }

        [Fact]
        public async Task SetTwoFactorEnabledAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            var user = new TestUser { UserName = "testuser" };
            var enabled = true;
            var error = new IdentityError { Description = "Set two-factor failed." };
            var userManager = SetTwoFactorEnabledAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetTwoFactorEnabledAsync(user, enabled);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Set two-factor failed.");
            userManager.Verify(um => um.SetTwoFactorEnabledAsync(user, enabled), Times.Once);
        }

        [Fact]
        public async Task SetTwoFactorEnabledAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var enabled = true;
            var userManager = SetTwoFactorEnabledAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetTwoFactorEnabledAsync(user, enabled);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.SetTwoFactorEnabledAsync(user, enabled), Times.Once);
        }

        [Fact]
        public async Task SetTwoFactorEnabledAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var enabled = true;
            var userManager = SetTwoFactorEnabledAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SetTwoFactorEnabledAsync(null!, enabled));
        }
    }
}