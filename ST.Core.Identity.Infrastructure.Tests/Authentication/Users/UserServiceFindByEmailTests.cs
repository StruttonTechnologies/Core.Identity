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
        public async Task FindByEmailAsync_ReturnsUser_WhenUserFound()
        {
            // Arrange
            var email = "testuser@example.com";
            var user = new TestUser { Email = email };
            var userManager = FindByEmailAsyncMock.WithUser(user);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.FindByEmailAsync(email);

            // Assert
            Assert.Equal(user, result);
            userManager.Verify(um => um.FindByEmailAsync(email), Times.Once);
        }

        [Fact]
        public async Task FindByEmailAsync_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            var email = "notfound@example.com";
            var userManager = FindByEmailAsyncMock.WithNull();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.FindByEmailAsync(email);

            // Assert
            Assert.Null(result);
            userManager.Verify(um => um.FindByEmailAsync(email), Times.Once);
        }

        [Fact]
        public async Task FindByEmailAsync_ReturnsNull_WhenUserManagerThrows()
        {
            // Arrange
            var email = "error@example.com";
            var userManager = FindByEmailAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.FindByEmailAsync(email);

            // Assert
            Assert.Null(result);
            userManager.Verify(um => um.FindByEmailAsync(email), Times.Once);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task FindByEmailAsync_ThrowsForInvalidArguments(string? email, Type expectedException)
        {
            // Arrange
            var userManager = FindByEmailAsyncMock.WithNull();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act & Assert
            await Assert.ThrowsAsync(expectedException, () => service.FindByEmailAsync(email!));
        }
    }
}