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
        public async Task SetLockoutEndDateAsync_ReturnsSuccess_WhenSetSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var lockoutEnd = DateTimeOffset.UtcNow.AddDays(1);
            var userManager = SetLockoutEndDateAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetLockoutEndDateAsync(user, lockoutEnd);

            Assert.True(result.Succeeded);
            userManager.Verify(um => um.SetLockoutEndDateAsync(user, lockoutEnd), Times.Once);
        }

        [Fact]
        public async Task SetLockoutEndDateAsync_ReturnsFailed_WhenUserManagerReturnsFailed()
        {
            var user = new TestUser { UserName = "testuser" };
            var lockoutEnd = DateTimeOffset.UtcNow.AddDays(1);
            var error = new IdentityError { Description = "Set lockout end date failed." };
            var userManager = SetLockoutEndDateAsyncMock.WithFailure(error.Description);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetLockoutEndDateAsync(user, lockoutEnd);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Set lockout end date failed.");
            userManager.Verify(um => um.SetLockoutEndDateAsync(user, lockoutEnd), Times.Once);
        }

        [Fact]
        public async Task SetLockoutEndDateAsync_ReturnsFailed_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var endDate = DateTimeOffset.UtcNow.AddDays(1);
            var userManager = SetLockoutEndDateAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.SetLockoutEndDateAsync(user, endDate);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description.Contains("Simulated failure"));
            userManager.Verify(um => um.SetLockoutEndDateAsync(user, endDate), Times.Once);
        }

        [Fact]
        public async Task SetLockoutEndDateAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var lockoutEnd = DateTimeOffset.UtcNow.AddDays(1);
            var userManager = SetLockoutEndDateAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SetLockoutEndDateAsync(null!, lockoutEnd));
        }
    }
}