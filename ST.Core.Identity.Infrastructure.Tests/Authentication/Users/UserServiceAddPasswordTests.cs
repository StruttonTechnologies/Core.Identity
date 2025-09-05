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
        public async Task AddPasswordAsync_ReturnsSuccess_WhenPasswordAdded()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var password = "SecureP@ssword123";

            var userManager = AddPasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.AddPasswordAsync(user, password);

            // Assert
            Assert.True(result.Succeeded);
            userManager.Verify(um => um.AddPasswordAsync(user, password), Times.Once);
        }

        [Fact]
        public async Task AddPasswordAsync_ReturnsFailure_WhenPasswordAddFails()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var password = "SecureP@ssword123";

            var userManager = AddPasswordAsyncMock.WithFailure("Password add failed.");
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.AddPasswordAsync(user, password);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Password add failed.");
            userManager.Verify(um => um.AddPasswordAsync(user, password), Times.Once);
        }

        [Theory]
        [InlineData(null, "ValidPassword", typeof(ArgumentNullException))]
        [InlineData("user", null, typeof(ArgumentException))]
        [InlineData("user", "", typeof(ArgumentException))]
        public async Task AddPasswordAsync_ThrowsForInvalidArguments(string? userType, string? password, Type expectedException)
        {
            var userManager = AddPasswordAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            await Assert.ThrowsAsync(expectedException, () => service.AddPasswordAsync(user!, password!));
        }
    }
}