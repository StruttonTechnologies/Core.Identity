using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.SetTwoFactorEnabledAsync"/>.
    /// Validates two-factor enablement logic, input validation, and exception handling.
    /// </summary>
    public class SetTwoFactorEnabledAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that enabling two-factor authentication for a valid user succeeds.
        /// </summary>
        [Fact]
        public async Task SetTwoFactorEnabledAsync_EnableTwoFactor_Succeeds()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.SetTwoFactorEnabledAsync(user, true);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.True(updatedUser!.TwoFactorEnabled);
        }

        /// <summary>
        /// Verifies that disabling two-factor authentication for a valid user succeeds.
        /// </summary>
        [Fact]
        public async Task SetTwoFactorEnabledAsync_DisableTwoFactor_Succeeds()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetTwoFactorEnabledAsync(user, true); // Pre-enable

            var result = await Service.SetTwoFactorEnabledAsync(user, false);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.False(updatedUser!.TwoFactorEnabled);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task SetTwoFactorEnabledAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.SetTwoFactorEnabledAsync(null!, true));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during two-factor enablement returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task SetTwoFactorEnabledAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.SetTwoFactorEnabledAsync(user, true);

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}