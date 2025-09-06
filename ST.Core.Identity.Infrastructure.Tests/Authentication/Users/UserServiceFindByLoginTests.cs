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
        public async Task FindByLoginAsync_ReturnsUser_WhenUserFound()
        {
            var loginProvider = "Google";
            var providerKey = "provider-key";
            var user = new TestUser { UserName = "testuser" };
            var userManager = FindByLoginAsyncMock.WithUser(user);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByLoginAsync(loginProvider, providerKey);

            Assert.Equal(user, result);
            userManager.Verify(um => um.FindByLoginAsync(loginProvider, providerKey), Times.Once);
        }

        [Fact]
        public async Task FindByLoginAsync_ReturnsNull_WhenUserNotFound()
        {
            var loginProvider = "Google";
            var providerKey = "notfound-key";
            var userManager = FindByLoginAsyncMock.WithNull();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByLoginAsync(loginProvider, providerKey);

            Assert.Null(result);
            userManager.Verify(um => um.FindByLoginAsync(loginProvider, providerKey), Times.Once);
        }

        [Fact]
        public async Task FindByLoginAsync_ReturnsNull_WhenUserManagerThrows()
        {
            var loginProvider = "Google";
            var providerKey = "error-key";
            var userManager = FindByLoginAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.FindByLoginAsync(loginProvider, providerKey);

            Assert.Null(result);
            userManager.Verify(um => um.FindByLoginAsync(loginProvider, providerKey), Times.Once);
        }

        [Theory]
        [InlineData(null, "key", typeof(ArgumentException))]
        [InlineData("", "key", typeof(ArgumentException))]
        [InlineData("provider", null, typeof(ArgumentException))]
        [InlineData("provider", "", typeof(ArgumentException))]
        public async Task FindByLoginAsync_ThrowsForInvalidArguments(string? loginProvider, string? providerKey, Type expectedException)
        {
            var userManager = FindByLoginAsyncMock.WithNull();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync(expectedException, () => service.FindByLoginAsync(loginProvider!, providerKey!));
        }
    }
}