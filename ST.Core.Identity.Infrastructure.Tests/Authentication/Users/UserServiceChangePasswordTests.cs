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
        public async Task ChangePasswordAsync_ReturnsSuccess_WhenPasswordChanged()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var currentPassword = "OldP@ssword123";
            var newPassword = "NewP@ssword456";
            var userManager = ChangePasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.ChangePasswordAsync(user, currentPassword, newPassword);

            // Assert
            Assert.True(result.Succeeded);
            userManager.Verify(m => m.ChangePasswordAsync(user, currentPassword, newPassword), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_ThrowsExceptionWithErrorMessage_WhenPasswordChangeFails()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var currentPassword = "OldP@ssword123";
            var newPassword = "NewP@ssword456";
            var errorDescription = "Password change failed.";
            var userManager = ChangePasswordAsyncMock.WithFailure(errorDescription);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.ChangePasswordAsync(user, currentPassword, newPassword);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(errorDescription, result.Errors.First().Description);
            userManager.Verify(m => m.ChangePasswordAsync(user, currentPassword, newPassword), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_ReturnsFailedResultWithExceptionMessage_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var currentPassword = "OldP@ssword123";
            var newPassword = "NewP@ssword456";
            var userManager = ChangePasswordAsyncMock.WithCustomResult((_, _, _) => throw new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.ChangePasswordAsync(user, currentPassword, newPassword);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Simulated failure", result.Errors.First().Description);
            userManager.Verify(m => m.ChangePasswordAsync(user, currentPassword, newPassword), Times.Once);
        }

        [Theory]
        [InlineData(null, "old", "new", typeof(ArgumentNullException))]
        [InlineData("user", null, "new", typeof(ArgumentNullException))]
        [InlineData("user", "", "new", typeof(ArgumentException))]
        [InlineData("user", "old", null, typeof(ArgumentNullException))]
        [InlineData("user", "old", "", typeof(ArgumentException))]
        public async Task ChangePasswordAsync_ThrowsForInvalidArguments(string? userType, string? currentPassword, string? newPassword, Type expectedException)
        {
            // Arrange
            var userManager = ChangePasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            // Act & Assert
            await Assert.ThrowsAsync(expectedException, () => service.ChangePasswordAsync(user!, currentPassword!, newPassword!));
        }
    }
}