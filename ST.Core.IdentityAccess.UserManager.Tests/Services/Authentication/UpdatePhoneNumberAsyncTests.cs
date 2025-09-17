using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Validators;
using ST.Core.IdentityAccess.Fakes.UserManager;
using ST.Core.IdentityAccess.Fakes.UserManager.Providers;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
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
            var tokenProvider = new FakeTwoFactorTokenProvider();
            UserManager.RegisterTokenProvider("Phone", tokenProvider);

            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            UserManager.UserValidators.Clear();
            UserManager.UserValidators.Add(new PhoneNumberFormatValidator());

            var result = await Service.UpdatePhoneNumberAsync(user, "+15551234567");

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Equal("+15551234567", updatedUser!.PhoneNumber);
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

            UserManager.UserValidators.Clear();
            UserManager.UserValidators.Add(new PhoneNumberFormatValidator());


            var result = await Service.UpdatePhoneNumberAsync(user, "invalid-phone");

            Assert.False(result.Succeeded);
            Assert.Contains("Phone number must be in valid internation", string.Join(", ", result.Errors.Select(e => e.Description)));
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
            Assert.Contains("User not found in store.", result.Errors.First().Description);
        }
    }
}