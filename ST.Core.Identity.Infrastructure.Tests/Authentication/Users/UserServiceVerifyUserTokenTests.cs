using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Testing.Toolkit.Models;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Tokens;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task VerifyUserTokenAsync_ReturnsTrue_WhenTokenValid()
        {
            // Arrange
            var user = new TestUser();
            var tokenProvider = "provider";
            var purpose = "purpose";
            var token = "token";
            var userManager = VerifyUserTokenAsyncMock.WithTrue();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.VerifyUserTokenAsync(user, tokenProvider, purpose, token);

            // Assert
            Assert.True(result);
            userManager.Verify(m => m.VerifyUserTokenAsync(user, tokenProvider, purpose, token), Times.Once);
        }

        [Fact]
        public async Task VerifyUserTokenAsync_ReturnsFalse_WhenTokenInvalid()
        {
            // Arrange
            var user = new TestUser();
            var tokenProvider = "provider";
            var purpose = "purpose";
            var token = "invalid";
            var userManager = VerifyUserTokenAsyncMock.WithFalse();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.VerifyUserTokenAsync(user, tokenProvider, purpose, token);

            // Assert
            Assert.False(result);
            userManager.Verify(m => m.VerifyUserTokenAsync(user, tokenProvider, purpose, token), Times.Once);
        }

        [Fact]
        public async Task VerifyUserTokenAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            // Arrange
            var user = new TestUser();
            var tokenProvider = "provider";
            var purpose = "purpose";
            var token = "token";
            var userManager = VerifyUserTokenAsyncMock.WithCustomResult((_, _, _, _) => throw new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.VerifyUserTokenAsync(user, tokenProvider, purpose, token);

            // Assert
            Assert.False(result);
            userManager.Verify(m => m.VerifyUserTokenAsync(user, tokenProvider, purpose, token), Times.Once);
        }

        [Theory]
        [InlineData(null, "provider", "purpose", "token", typeof(ArgumentNullException))]
        [InlineData("user", null, "purpose", "token", typeof(ArgumentNullException))]
        [InlineData("user", "", "purpose", "token", typeof(ArgumentException))]
        [InlineData("user", "provider", null, "token", typeof(ArgumentNullException))]
        [InlineData("user", "provider", "", "token", typeof(ArgumentException))]
        [InlineData("user", "provider", "purpose", null, typeof(ArgumentNullException))]
        [InlineData("user", "provider", "purpose", "", typeof(ArgumentException))]
        public async Task VerifyUserTokenAsync_ThrowsForInvalidArguments(string? userType, string? tokenProvider, string? purpose, string? token, Type expectedException)
        {
            // Arrange
            var userManager = VerifyUserTokenAsyncMock.WithTrue();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser();

            // Act & Assert
            await Assert.ThrowsAsync(expectedException, () => service.VerifyUserTokenAsync(user!, tokenProvider!, purpose!, token!));
        }
    }
}