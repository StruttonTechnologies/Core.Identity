using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Testing.Toolkit.Models;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.TwoFactor;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task VerifyTwoFactorTokenAsync_ReturnsTrue_WhenTokenValid()
        {
            // Arrange
            var user = new TestUser();
            var tokenProvider = "provider";
            var token = "token";
            var userManager = VerifyTwoFactorTokenAsyncMock.WithTrue();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.VerifyTwoFactorTokenAsync(user, tokenProvider, token);

            // Assert
            Assert.True(result);
            userManager.Verify(m => m.VerifyTwoFactorTokenAsync(user, tokenProvider, token), Times.Once);
        }

        [Fact]
        public async Task VerifyTwoFactorTokenAsync_ReturnsFalse_WhenTokenInvalid()
        {
            // Arrange
            var user = new TestUser();
            var tokenProvider = "provider";
            var token = "invalid";
            var userManager = VerifyTwoFactorTokenAsyncMock.WithFalse();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.VerifyTwoFactorTokenAsync(user, tokenProvider, token);

            // Assert
            Assert.False(result);
            userManager.Verify(m => m.VerifyTwoFactorTokenAsync(user, tokenProvider, token), Times.Once);
        }

        [Fact]
        public async Task VerifyTwoFactorTokenAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser();
            var tokenProvider = "provider";
            var token = "token";
            var userManager = VerifyTwoFactorTokenAsyncMock.WithCustomResult((_, _, _) => throw new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.VerifyTwoFactorTokenAsync(user, tokenProvider, token);

            // Assert
            Assert.False(result);
            userManager.Verify(m => m.VerifyTwoFactorTokenAsync(user, tokenProvider, token), Times.Once);
        }

        [Theory]
        [InlineData(null, "provider", "token", typeof(ArgumentNullException))]
        [InlineData("user", null, "token", typeof(ArgumentNullException))]
        [InlineData("user", "", "token", typeof(ArgumentException))]
        [InlineData("user", "provider", null, typeof(ArgumentNullException))]
        [InlineData("user", "provider", "", typeof(ArgumentException))]
        public async Task VerifyTwoFactorTokenAsync_ThrowsForInvalidArguments(string? userType, string? tokenProvider, string? token, Type expectedException)
        {
            // Arrange
            var userManager = VerifyTwoFactorTokenAsyncMock.WithTrue();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser();

            // Act & Assert
            await Assert.ThrowsAsync(expectedException, () => service.VerifyTwoFactorTokenAsync(user!, tokenProvider!, token!));
        }
    }
}