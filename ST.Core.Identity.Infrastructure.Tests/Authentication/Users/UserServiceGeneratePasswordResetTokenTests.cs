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
        public async Task GeneratePasswordResetTokenAsync_ReturnsToken_WhenSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var expectedToken = "reset-token";
            var userManager = GeneratePasswordResetTokenAsyncMock.WithToken(expectedToken);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GeneratePasswordResetTokenAsync(user);

            Assert.Equal(expectedToken, result);
            userManager.Verify(um => um.GeneratePasswordResetTokenAsync(user), Times.Once);
        }

        [Fact]
        public async Task GeneratePasswordResetTokenAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GeneratePasswordResetTokenAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GeneratePasswordResetTokenAsync(user);

            Assert.Null(result);
            userManager.Verify(um => um.GeneratePasswordResetTokenAsync(user), Times.Once);
        }

        [Fact]
        public async Task GeneratePasswordResetTokenAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var userManager = GeneratePasswordResetTokenAsyncMock.WithToken("any-token");
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GeneratePasswordResetTokenAsync(null!));
        }
    }
}