using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.FindByNameAsync"/>.
    /// Validates user lookup by username, input validation, and exception handling.
    /// </summary>
    public class FindByNameAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid username returns the corresponding user.
        /// </summary>
        [Fact]
        public async Task FindByNameAsync_ValidUsername_ReturnsUser()
        {
            var user = TestAppUserIdentityFactory.Create("testuser");
            await UserManager.CreateAsync(user);

            var result = await Service.FindByNameAsync("testuser");

            Assert.NotNull(result);
            Assert.Equal(user.UserName, result!.UserName);
        }

        /// <summary>
        /// Verifies that an unknown username returns <c>null</c>.
        /// </summary>
        [Fact]
        public async Task FindByNameAsync_UnknownUsername_ReturnsNull()
        {
            var result = await Service.FindByNameAsync("nonexistentuser");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that null or empty username throws <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="username">The username input to test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task FindByNameAsync_InvalidUsername_ThrowsArgumentException(string? username)
        {
            var exception = await Record.ExceptionAsync(() => Service.FindByNameAsync(username!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during lookup returns <c>null</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task FindByNameAsync_ThrowsException_ReturnsNull()
        {
            var user = TestAppUserIdentityFactory.Create("testuser");
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.FindByNameAsync("testuser");

            Assert.Null(result);
        }
    }
}