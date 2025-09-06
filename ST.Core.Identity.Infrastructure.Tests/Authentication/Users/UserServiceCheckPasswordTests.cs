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
        public async Task CheckPasswordAsync_ReturnsTrue_WhenPasswordIsValid()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var password = "ValidP@ssword123";
            var userManager = CheckPasswordAsyncMock.WithTrue();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.CheckPasswordAsync(user, password);

            // Assert
            Assert.True(result);
            userManager.Verify(um => um.CheckPasswordAsync(user, password), Times.Once);
        }

        [Fact]
        public async Task CheckPasswordAsync_ReturnsFalse_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var password = "InvalidP@ssword";
            var userManager = CheckPasswordAsyncMock.WithFalse();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.CheckPasswordAsync(user, password);

            // Assert
            Assert.False(result);
            userManager.Verify(um => um.CheckPasswordAsync(user, password), Times.Once);
        }

        [Fact]
        public async Task CheckPasswordAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var password = "AnyPassword";
            var userManager = CheckPasswordAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.CheckPasswordAsync(user, password);

            // Assert
            Assert.False(result);
            userManager.Verify(um => um.CheckPasswordAsync(user, password), Times.Once);
        }

        [Theory]
        [InlineData(null, "ValidPassword", typeof(ArgumentNullException))]
        [InlineData("user", null, typeof(ArgumentException))]
        [InlineData("user", "", typeof(ArgumentException))]
        public async Task CheckPasswordAsync_ThrowsForInvalidArguments(string? userType, string? password, Type expectedException)
        {
            // Arrange
            var userManager = CheckPasswordAsyncMock.WithTrue();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            // Act & Assert
            await Assert.ThrowsAsync(expectedException, () => service.CheckPasswordAsync(user!, password!));
        }
    }
}