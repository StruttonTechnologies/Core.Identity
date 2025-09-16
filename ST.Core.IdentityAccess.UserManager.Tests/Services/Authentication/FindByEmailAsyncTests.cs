using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.FindByEmailAsync"/>.
    /// Validates user lookup by email, input validation, and exception handling.
    /// </summary>
    public class FindByEmailAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid email returns the corresponding user.
        /// </summary>
        [Fact]
        public async Task FindByEmailAsync_ValidEmail_ReturnsUser()
        {
            var user = TestAppUserIdentityFactory.Create("user@example.com");
            await UserManager.CreateAsync(user);

            var result = await Service.FindByEmailAsync(user.Email!);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result!.Email);
        }

        /// <summary>
        /// Verifies that an unknown email returns <c>null</c>.
        /// </summary>
        [Fact]
        public async Task FindByEmailAsync_UnknownEmail_ReturnsNull()
        {
            var result = await Service.FindByEmailAsync("unknown@example.com");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that null or empty email throws <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="email">The email input to test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task FindByEmailAsync_InvalidEmail_ThrowsArgumentException(string? email)
        {
            var exception = await Record.ExceptionAsync(() => Service.FindByEmailAsync(email!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during lookup returns <c>null</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task FindByEmailAsync_ThrowsException_ReturnsNull()
        {
            var user = TestAppUserIdentityFactory.Create("user@example.com");
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.FindByEmailAsync(user.Email!);

            Assert.Null(result);
        }
    }
}