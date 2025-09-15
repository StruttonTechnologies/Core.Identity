using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.RemovePasswordAsync"/>.
    /// Validates password removal logic, input validation, and exception handling.
    /// </summary>
    public class RemovePasswordAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid user with a password can have it removed successfully.
        /// </summary>
        [Fact]
        public async Task RemovePasswordAsync_ValidUserWithPassword_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, "SecureP@ssword123");

            var result = await Service.RemovePasswordAsync(user);

            Assert.True(result.Succeeded);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task RemovePasswordAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.RemovePasswordAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during password removal returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task RemovePasswordAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, "SecureP@ssword123");
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.RemovePasswordAsync(user);

            Assert.False(result.Succeeded);
            var error = result.Errors.FirstOrDefault();
            Assert.NotNull(error);
            Assert.Contains("Exception:", error.Description);
        }
    }
}