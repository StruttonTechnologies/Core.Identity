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
        public async Task AccessFailedAsync_ReturnsSuccess_WhenAccessFailureRecorded()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser", Id = "some-id" };
            var userManager = AccessFailedAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.AccessFailedAsync(user);

            // Assert
            Assert.True(result.Succeeded);
            userManager.Verify(um => um.AccessFailedAsync(user), Times.Once);
        }

        [Fact]
        public async Task AccessFailedAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser", Id = "some-id" };
            var userManager = AccessFailedAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.AccessFailedAsync(user);

            // Assert
            Assert.False(result?.Succeeded);
            Assert.Contains(result?.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.AccessFailedAsync(user), Times.Once);
        }

        [Fact]
        public async Task AccessFailedAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            var userManager = AccessFailedAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AccessFailedAsync(null!));
        }
    }
}