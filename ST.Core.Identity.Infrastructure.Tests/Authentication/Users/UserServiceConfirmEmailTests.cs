using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Tokens;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task ConfirmEmailAsync_ReturnsSuccess_WhenEmailConfirmed()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var token = "valid-token";
            var userManager = ConfirmEmailAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.ConfirmEmailAsync(user, token);

            // Assert
            Assert.True(result.Succeeded);
            userManager.Verify(um => um.ConfirmEmailAsync(user, token), Times.Once);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var token = "invalid-token";
            var error = new IdentityError { Description = "Email confirmation failed." };
            var userManager = ConfirmEmailAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.ConfirmEmailAsync(user, token);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Email confirmation failed.");
            userManager.Verify(um => um.ConfirmEmailAsync(user, token), Times.Once);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var token = "any-token";
            var userManager = ConfirmEmailAsyncMock.WithCustomResult((u, t) => throw new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.ConfirmEmailAsync(user, token);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Simulated failure", result.Errors.First().Description);
            userManager.Verify(um => um.ConfirmEmailAsync(user, token), Times.Once);
        }

        [Theory]
        [InlineData(null, "valid-token", typeof(ArgumentNullException))]
        [InlineData("user", null, typeof(ArgumentException))]
        [InlineData("user", "", typeof(ArgumentException))]
        public async Task ConfirmEmailAsync_ThrowsForInvalidArguments(string? userType, string? token, Type expectedException)
        {
            // Arrange
            var userManager = ConfirmEmailAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            // Act & Assert
            await Assert.ThrowsAsync(expectedException, () => service.ConfirmEmailAsync(user!, token!));
        }
    }
}