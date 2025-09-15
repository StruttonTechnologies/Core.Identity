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
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.UpdatePhoneNumberAsync"/>.
    /// Validates phone number update logic, token generation, input validation, and exception handling.
    /// </summary>
    public class UpdatePhoneNumberAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid phone number update returns a successful result.
        /// </summary>
        [Fact]
        public async Task UpdatePhoneNumberAsync_ValidPhoneNumber_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.UpdatePhoneNumberAsync(user, "555-1234");

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Equal("555-1234", updatedUser!.PhoneNumber);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdatePhoneNumberAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.UpdatePhoneNumberAsync(null!, "555-1234"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null phone number throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdatePhoneNumberAsync_NullPhoneNumber_ThrowsArgumentNullException()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.UpdatePhoneNumberAsync(user, null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an invalid phone number format returns a failed result and logs a warning.
        /// </summary>
        [Fact]
        public async Task UpdatePhoneNumberAsync_InvalidPhoneNumber_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.UpdatePhoneNumberAsync(user, "invalid-phone");

            Assert.False(result.Succeeded);
            Assert.Contains("Invalid", string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        /// <summary>
        /// Verifies that an exception during token or phone number change returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task UpdatePhoneNumberAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.UpdatePhoneNumberAsync(user, "555-1234");

            Assert.False(result.Succeeded);
            Assert.Contains("Exception occurred:", result.Errors.First().Description);
        }
    }
}