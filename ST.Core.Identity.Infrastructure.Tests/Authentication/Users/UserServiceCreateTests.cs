using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager;
using ST.Core.Identity.Testing.Toolkit.Mocks.UserManager.User;
using ST.Core.Identity.Testing.Toolkit.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    public partial class TestUserServiceTests
    {
        [Fact]
        public async Task CreateAsync_ReturnsUser_WhenUserCreated()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser" };
            var password = "SecureP@ssword123";

            var userManager = CreateAsyncMock.WithSuccess();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManager.Object, logger.Object);

            // Act
            var result = await service.CreateAsync(user, password);

            // Assert
            Assert.Equal(user, result);
            userManager.Verify(um => um.CreateAsync(user), Times.Once);
            userManager.Verify(um => um.AddPasswordAsync(user, password), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ThrowsExceptionWithErrorMessage_WhenUserCreationFails()
        {
            // Arrange
            var user = new TestUser { UserName = "testuser", Id = "some-id" };
            var password = "SecureP@ssword123";

            var userManagerMock = CreateAsyncMock.WithFailure("Password too weak");
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManagerMock.Object, logger.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CreateAsync(user, password));

            Assert.Contains("Password too weak", ex.Message);
            userManagerMock.Verify(m => m.CreateAsync(user), Times.Once);
            userManagerMock.Verify(m => m.AddPasswordAsync(It.IsAny<TestUser>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(null, "ValidPassword", typeof(ArgumentNullException))]
        [InlineData("user", null, typeof(ArgumentNullException))]
        [InlineData("user", "", typeof(ArgumentException))]
        public async Task CreateAsync_ThrowsForInvalidArguments(string? userType, string? password, Type expectedException)
        {
            // Arrange
            var userManagerMock = MockUserManagerFactory.Create();
            var logger = new Mock<ILogger<UserIdentityService<TestUser>>>();
            var service = new TestUserIdentityService(userManagerMock.Object, logger.Object);

            TestUser? user = userType == null ? null : new TestUser { UserName = "testuser" };

            // Act & Assert
            await Assert.ThrowsAsync(expectedException, () => service.CreateAsync(user!, password!));
        }
    }
}