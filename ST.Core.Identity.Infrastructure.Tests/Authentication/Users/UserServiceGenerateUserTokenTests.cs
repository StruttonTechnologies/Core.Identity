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
        public async Task GenerateUserTokenAsync_ReturnsToken_WhenSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var tokenProvider = "provider";
            var purpose = "purpose";
            var expectedToken = "user-token";
            var userManager = GenerateUserTokenAsyncMock.WithToken(expectedToken);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GenerateUserTokenAsync(user, tokenProvider, purpose);

            Assert.Equal(expectedToken, result);
            userManager.Verify(um => um.GenerateUserTokenAsync(user, tokenProvider, purpose), Times.Once);
        }

        [Fact]
        public async Task GenerateUserTokenAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var loginProvider = "provider";
            var tokenName = "token";
            var userManager = GenerateUserTokenAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GenerateUserTokenAsync(user, loginProvider, tokenName);

            Assert.Null(result);
            userManager.Verify(um => um.GenerateUserTokenAsync(user, loginProvider, tokenName), Times.Once);
        }

        [Theory]
        [InlineData(null, "provider", "purpose", typeof(ArgumentNullException))]
        [InlineData("user", null, "purpose", typeof(ArgumentException))]
        [InlineData("user", "", "purpose", typeof(ArgumentException))]
        [InlineData("user", "provider", null, typeof(ArgumentException))]
        [InlineData("user", "provider", "", typeof(ArgumentException))]
        public async Task GenerateUserTokenAsync_ThrowsForInvalidArguments(string? userType, string? tokenProvider, string? purpose, Type expectedException)
        {
            var userManager = GenerateUserTokenAsyncMock.WithToken("any-token");
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            await Assert.ThrowsAsync(expectedException, () => service.GenerateUserTokenAsync(user!, tokenProvider!, purpose!));
        }
    }
}