using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        /// <summary>
        /// Should return IdentityResult.Success when user creation succeeds.
        /// </summary>
        [Fact]
        public async Task CreateNoPasswordAsync_ReturnsSuccess_WhenUserCreated()
        {
            // Arrange
            var user = new TestUser();
            var userManager = CreateAsyncMock.CreateNoPasswordWithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.CreateNoPasswordAsync(user);

            // Assert
            Assert.True(result.Succeeded);
            userManager.Verify(m => m.CreateAsync(user), Times.Once);
        }

        /// <summary>
        /// Should return IdentityResult.Failed when UserManager returns a failed result.
        /// </summary>
        [Fact]
        public async Task CreateNoPasswordAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            // Arrange
            var user = new TestUser();
            var errorDescription = "User creation failed.";
            var userManager = CreateAsyncMock.CreateNoPasswordWithFailure(errorDescription);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.CreateNoPasswordAsync(user);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == errorDescription);
            userManager.Verify(m => m.CreateAsync(user), Times.Once);
        }

        /// <summary>
        /// Should return IdentityResult.Failed when UserManager throws an exception.
        /// </summary>
        [Fact]
        public async Task CreateNoPasswordAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser();
            var userManager = CreateAsyncMock.WithCustomResult(_ => throw new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.CreateNoPasswordAsync(user);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(m => m.CreateAsync(user), Times.Once);
        }

        /// <summary>
        /// Should throw ArgumentNullException when user argument is null.
        /// </summary>
        [Fact]
        public async Task CreateNoPasswordAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            var userManager = CreateAsyncMock.CreateNoPasswordWithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateNoPasswordAsync(null!));
        }
    }
}