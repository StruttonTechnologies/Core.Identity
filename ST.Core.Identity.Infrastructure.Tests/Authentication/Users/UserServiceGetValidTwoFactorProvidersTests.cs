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
        public async Task GetValidTwoFactorProvidersAsync_ReturnsProviders_WhenSuccessful()
        {
            var user = new TestUser { UserName = "testuser" };
            var expectedProviders = new List<string> { "Email", "Phone" };
            var userManager = GetValidTwoFactorProvidersAsyncMock.WithProviders(expectedProviders);
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetValidTwoFactorProvidersAsync(user);

            Assert.Equal(expectedProviders, result);
            userManager.Verify(um => um.GetValidTwoFactorProvidersAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetValidTwoFactorProvidersAsync_ReturnsEmptyList_WhenUserManagerThrows()
        {
            var user = new TestUser { UserName = "testuser" };
            var userManager = GetValidTwoFactorProvidersAsyncMock.WithException(new InvalidOperationException("Simulated failure"));
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            var result = await service.GetValidTwoFactorProvidersAsync(user);

            Assert.Empty(result);
            userManager.Verify(um => um.GetValidTwoFactorProvidersAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetValidTwoFactorProvidersAsync_ThrowsArgumentNullException_WhenUserIsNull()
        {
            var userManager = GetValidTwoFactorProvidersAsyncMock.WithProviders(new List<string> { "Email" });
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetValidTwoFactorProvidersAsync(null!));
        }
    }
}