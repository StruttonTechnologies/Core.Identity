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
        public async Task RemovePasswordAsync_ReturnsSuccess_WhenPasswordRemoved()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = RemovePasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.RemovePasswordAsync(user);

            Assert.True(result.Succeeded);
            userManager.Verify(um => um.RemovePasswordAsync(user), Times.Once);
        }

        [Fact]
        public async Task RemovePasswordAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            var user = new TestUser { UserName = "testuser" };
            var error = new IdentityError { Description = "Password removal failed." };
            var userManager = RemovePasswordAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.RemovePasswordAsync(user);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Password removal failed.");
            userManager.Verify(um => um.RemovePasswordAsync(user), Times.Once);
        }

        [Fact]
        public async Task RemovePasswordAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = RemovePasswordAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.RemovePasswordAsync(user);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.RemovePasswordAsync(user), Times.Once);
        }

        [Fact]
        public async Task RemovePasswordAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var userManager = RemovePasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.RemovePasswordAsync(null!));
        }
    }
}