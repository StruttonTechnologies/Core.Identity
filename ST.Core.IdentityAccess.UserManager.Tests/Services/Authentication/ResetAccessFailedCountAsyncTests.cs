using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.ResetAccessFailedCountAsync"/>.
    /// Validates reset logic, input validation, and exception handling.
    /// </summary>
    public class ResetAccessFailedCountAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a user with failed access attempts has their count reset successfully.
        /// </summary>
        [Fact]
        public async Task ResetAccessFailedCountAsync_UserWithFailures_ResetsCount()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AccessFailedAsync(user); // Simulate failure
            await UserManager.AccessFailedAsync(user);

            var result = await Service.ResetAccessFailedCountAsync(user);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id.ToString());
            Assert.Equal(0, updatedUser!.AccessFailedCount);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task ResetAccessFailedCountAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.ResetAccessFailedCountAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during reset returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task ResetAccessFailedCountAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AccessFailedAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.ResetAccessFailedCountAsync(user);

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}
