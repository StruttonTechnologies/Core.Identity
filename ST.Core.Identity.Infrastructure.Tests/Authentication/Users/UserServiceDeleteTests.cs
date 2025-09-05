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
        public async Task DeleteAsync_ReturnsTrue_WhenUserDeletedSuccessfully()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = DeleteAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.DeleteAsync(user, cancellationToken);

            // Assert
            Assert.True(result);
            userManager.Verify(um => um.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserManagerReturnsFailed()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = DeleteAsyncMock.WithFailure("User deletion failed.");
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.DeleteAsync(user, cancellationToken);

            // Assert
            Assert.False(result);
            userManager.Verify(um => um.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = DeleteAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.DeleteAsync(user, cancellationToken);

            // Assert
            Assert.False(result);
            userManager.Verify(um => um.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var userManager = DeleteAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeleteAsync(null!, cancellationToken));
        }
    }
}