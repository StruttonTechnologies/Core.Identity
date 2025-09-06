using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.TwoFactor;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task GenerateTwoFactorTokenAsync_ReturnsToken_WhenSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var tokenProvider = "provider";
            var expectedToken = "2fa-token";
            var userManager = GenerateTwoFactorTokenAsyncMock.WithToken(expectedToken);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GenerateTwoFactorTokenAsync(user, tokenProvider);

            Assert.Equal(expectedToken, result);
            userManager.Verify(um => um.GenerateTwoFactorTokenAsync(user, tokenProvider), Times.Once);
        }

        [Fact]
        public async Task GenerateTwoFactorTokenAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var tokenProvider = "provider";
            var userManager = GenerateTwoFactorTokenAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GenerateTwoFactorTokenAsync(user, tokenProvider);

            Assert.Null(result);
            userManager.Verify(um => um.GenerateTwoFactorTokenAsync(user, tokenProvider), Times.Once);
        }

        [Theory]
        [InlineData(null, "provider", typeof(ArgumentNullException))]
        [InlineData("user", null, typeof(ArgumentException))]
        [InlineData("user", "", typeof(ArgumentException))]
        public async Task GenerateTwoFactorTokenAsync_ThrowsForInvalidArguments(string? userType, string? tokenProvider, Type expectedException)
        {
            var userManager = GenerateTwoFactorTokenAsyncMock.WithToken("any-token");
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            await Assert.ThrowsAsync(expectedException, () => service.GenerateTwoFactorTokenAsync(user!, tokenProvider!));
        }
    }
}