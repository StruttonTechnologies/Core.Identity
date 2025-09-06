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
        public async Task GetAllUsersAsync_ReturnsAllUsers_WhenSuccessful()
        {
            var users = new List<TestUser>
            {
                new() { UserName = "user1" },
                new() { UserName = "user2" }
            };

            var userManager = GetAllUsersAsyncMock.WithUsers(users);
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetAllUsersAsync();

            Assert.Equal(users, result);
            userManager.VerifyGet(um => um.Users, Times.Once);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsEmpty_WhenUserManagerThrows()
        {
            var userManager = GetAllUsersAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<AuthenticationUserService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetAllUsersAsync();

            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<TestUser>>(result);
        }
    }
}