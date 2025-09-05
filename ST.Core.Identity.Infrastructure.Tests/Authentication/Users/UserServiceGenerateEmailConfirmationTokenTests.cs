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
        public async Task GenerateEmailConfirmationTokenAsync_ReturnsToken_WhenSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var expectedToken = "email-token";
            var userManager = GenerateEmailConfirmationTokenAsyncMock.WithToken(expectedToken);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GenerateEmailConfirmationTokenAsync(user);

            Assert.Equal(expectedToken, result);
            userManager.Verify(um => um.GenerateEmailConfirmationTokenAsync(user), Times.Once);
        }

        [Fact]
        public async Task GenerateEmailConfirmationTokenAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GenerateEmailConfirmationTokenAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GenerateEmailConfirmationTokenAsync(user);

            Assert.Null(result);
            userManager.Verify(um => um.GenerateEmailConfirmationTokenAsync(user), Times.Once);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        public async Task GenerateEmailConfirmationTokenAsync_ThrowsForInvalidArguments(TestUser? user, Type expectedException)
        {
            var userManager = GenerateEmailConfirmationTokenAsyncMock.WithToken("any-token");
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync(expectedException, () => service.GenerateEmailConfirmationTokenAsync(user!));
        }
    }
}