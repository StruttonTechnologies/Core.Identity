using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.SetLockoutEnabledAsync"/>.
    /// Validates lockout enablement logic, input validation, and exception handling.
    /// </summary>
    public class SetLockoutEnabledAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that enabling lockout for a valid user succeeds.
        /// </summary>
        [Fact]
        public async Task SetLockoutEnabledAsync_EnableLockout_Succeeds()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.SetLockoutEnabledAsync(user, true);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.True(updatedUser!.LockoutEnabled);
        }

        /// <summary>
        /// Verifies that disabling lockout for a valid user succeeds.
        /// </summary>
        [Fact]
        public async Task SetLockoutEnabledAsync_DisableLockout_Succeeds()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetLockoutEnabledAsync(user, true); // Pre-enable

            var result = await Service.SetLockoutEnabledAsync(user, false);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.False(updatedUser!.LockoutEnabled);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task SetLockoutEnabledAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.SetLockoutEnabledAsync(null!, true));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during lockout enablement returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task SetLockoutEnabledAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.SetLockoutEnabledAsync(user, true);

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}
