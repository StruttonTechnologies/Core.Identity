using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task IsEmailConfirmedAsync_ReturnsTrue_WhenEmailIsConfirmed()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = IsEmailConfirmedAsyncMock.WithValue(true);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.IsEmailConfirmedAsync(user, cancellationToken);

            Assert.True(result);
            userManager.Verify(um => um.IsEmailConfirmedAsync(user), Times.Once);
        }

        [Fact]
        public async Task IsEmailConfirmedAsync_ReturnsFalse_WhenEmailIsNotConfirmed()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = IsEmailConfirmedAsyncMock.WithValue(false);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.IsEmailConfirmedAsync(user, cancellationToken);

            Assert.False(result);
            userManager.Verify(um => um.IsEmailConfirmedAsync(user), Times.Once);
        }

        [Fact]
        public async Task IsEmailConfirmedAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = IsEmailConfirmedAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.IsEmailConfirmedAsync(user);

            Assert.False(result);
            userManager.Verify(um => um.IsEmailConfirmedAsync(user), Times.Once);
        }

        [Fact]
        public async Task IsEmailConfirmedAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var cancellationToken = CancellationToken.None;
            var userManager = IsEmailConfirmedAsyncMock.WithValue(true);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.IsEmailConfirmedAsync(null!, cancellationToken));
        }
    }
}