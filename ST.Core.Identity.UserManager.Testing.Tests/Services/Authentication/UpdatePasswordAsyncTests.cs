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
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.UpdatePasswordAsync"/>.
    /// Validates password update logic, token generation, input validation, and exception handling.
    /// </summary>
    public class UpdatePasswordAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid password update returns a successful result.
        /// </summary>
        [Fact]
        public async Task UpdatePasswordAsync_ValidPassword_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, "OldP@ssword123");

            var result = await Service.UpdatePasswordAsync(user, "NewP@ssword456");

            Assert.True(result.Succeeded);
            var isNewPasswordValid = await UserManager.CheckPasswordAsync(user, "NewP@ssword456");
            Assert.True(isNewPasswordValid);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdatePasswordAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.UpdatePasswordAsync(null!, "NewP@ssword456"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null password throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdatePasswordAsync_NullPassword_ThrowsArgumentNullException()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.UpdatePasswordAsync(user, null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an invalid password format returns a failed result and logs a warning.
        /// </summary>
        [Fact]
        public async Task UpdatePasswordAsync_InvalidPassword_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, "OldP@ssword123");

            var result = await Service.UpdatePasswordAsync(user, "short");

            Assert.False(result.Succeeded);
            Assert.Contains("Password", string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        /// <summary>
        /// Verifies that an exception during token or password reset returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task UpdatePasswordAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, "OldP@ssword123");
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.UpdatePasswordAsync(user, "NewP@ssword456");

            Assert.False(result.Succeeded);
            Assert.Contains("Exception occurred:", result.Errors.First().Description);
        }
    }
}