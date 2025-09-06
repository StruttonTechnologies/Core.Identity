using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.Lockout;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task IsLockedOutAsync_ReturnsTrue_WhenUserIsLockedOut()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = IsLockedOutAsyncMock.WithValue(true);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.IsLockedOutAsync(user, cancellationToken);

            Assert.True(result);
            userManager.Verify(um => um.IsLockedOutAsync(user), Times.Once);
        }

        [Fact]
        public async Task IsLockedOutAsync_ReturnsFalse_WhenUserIsNotLockedOut()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = IsLockedOutAsyncMock.WithValue(false);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.IsLockedOutAsync(user, cancellationToken);

            Assert.False(result);
            userManager.Verify(um => um.IsLockedOutAsync(user), Times.Once);
        }

        [Fact]
        public async Task IsLockedOutAsync_ReturnsFalse_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var cancellationToken = CancellationToken.None;
            var userManager = IsLockedOutAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.IsLockedOutAsync(user, cancellationToken);

            Assert.False(result);
            userManager.Verify(um => um.IsLockedOutAsync(user), Times.Once);
        }

        [Fact]
        public async Task IsLockedOutAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var cancellationToken = CancellationToken.None;
            var userManager = IsLockedOutAsyncMock.WithValue(true);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.IsLockedOutAsync(null!, cancellationToken));
        }
    }
}