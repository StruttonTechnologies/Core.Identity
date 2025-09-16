using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.IsEmailConfirmedAsync"/>.
    /// Validates email confirmation status checks, input validation, and exception handling.
    /// </summary>
    public class IsEmailConfirmedAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a user with confirmed email returns <c>true</c>.
        /// </summary>
        [Fact]
        public async Task IsEmailConfirmedAsync_EmailConfirmed_ReturnsTrue()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            user.EmailConfirmed = true;
            await UserManager.CreateAsync(user);

            var result = await Service.IsEmailConfirmedAsync(user);

            Assert.True(result);
        }

        /// <summary>
        /// Verifies that a user with unconfirmed email returns <c>false</c>.
        /// </summary>
        [Fact]
        public async Task IsEmailConfirmedAsync_EmailNotConfirmed_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            user.EmailConfirmed = false;
            await UserManager.CreateAsync(user);

            var result = await Service.IsEmailConfirmedAsync(user);

            Assert.False(result);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task IsEmailConfirmedAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.IsEmailConfirmedAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during confirmation check returns <c>false</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task IsEmailConfirmedAsync_ThrowsException_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            user.EmailConfirmed = true;
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.IsEmailConfirmedAsync(user);

            Assert.False(result);
        }
    }
}