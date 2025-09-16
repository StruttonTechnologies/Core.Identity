using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.UpdateEmailAsync"/>.
    /// Validates email update logic, token generation, input validation, and exception handling.
    /// </summary>
    public class UpdateEmailAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid email update returns a successful result.
        /// </summary>
        [Fact]
        public async Task UpdateEmailAsync_ValidEmail_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.UpdateEmailAsync(user, "updated@example.com");

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Equal("updated@example.com", updatedUser!.Email);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdateEmailAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.UpdateEmailAsync(null!, "updated@example.com"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null email throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdateEmailAsync_NullEmail_ThrowsArgumentNullException()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.UpdateEmailAsync(user, null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an invalid email format returns a failed result and logs a warning.
        /// </summary>
        [Fact]
        public async Task UpdateEmailAsync_InvalidEmail_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.UpdateEmailAsync(user, "invalid-email");

            Assert.False(result.Succeeded);
            Assert.Contains("Invalid", string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        /// <summary>
        /// Verifies that an exception during token or email change returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task UpdateEmailAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.UpdateEmailAsync(user, "updated@example.com");

            Assert.False(result.Succeeded);
            Assert.Contains("Exception occurred:", result.Errors.First().Description);
        }
    }
}