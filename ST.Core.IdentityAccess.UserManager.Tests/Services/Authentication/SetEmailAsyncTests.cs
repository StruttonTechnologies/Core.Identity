using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.SetEmailAsync"/>.
    /// Validates email assignment logic, input validation, and exception handling.
    /// </summary>
    public class SetEmailAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid email is set successfully for the user.
        /// </summary>
        [Fact]
        public async Task SetEmailAsync_ValidEmail_SetsSuccessfully()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.SetEmailAsync(user, "new.email@example.com");

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id.ToString());
            Assert.Equal("new.email@example.com", updatedUser!.Email);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task SetEmailAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.SetEmailAsync(null!, "email@example.com"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null or empty email throws <see cref="ArgumentException"/>.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task SetEmailAsync_InvalidEmail_ThrowsArgumentException(string? email)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.SetEmailAsync(user, email));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during email setting returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task SetEmailAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.SetEmailAsync(user, "new.email@example.com");

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}