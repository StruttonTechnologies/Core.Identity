using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.SetLockoutEndDateAsync"/>.
    /// Validates lockout end date assignment, input validation, and exception handling.
    /// </summary>
    public class SetLockoutEndDateAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that setting a future lockout end date succeeds.
        /// </summary>
        [Fact]
        public async Task SetLockoutEndDateAsync_ValidFutureDate_Succeeds()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(10);

            var result = await Service.SetLockoutEndDateAsync(user, lockoutEnd);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Equal(lockoutEnd.UtcDateTime, updatedUser!.LockoutEnd?.UtcDateTime);
        }

        /// <summary>
        /// Verifies that clearing the lockout end date succeeds.
        /// </summary>
        [Fact]
        public async Task SetLockoutEndDateAsync_ClearLockout_Succeeds()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(5)); // Pre-lock

            var result = await Service.SetLockoutEndDateAsync(user, null);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Null(updatedUser!.LockoutEnd);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task SetLockoutEndDateAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.SetLockoutEndDateAsync(null!, DateTimeOffset.UtcNow));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during lockout end date assignment returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task SetLockoutEndDateAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(5));

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}