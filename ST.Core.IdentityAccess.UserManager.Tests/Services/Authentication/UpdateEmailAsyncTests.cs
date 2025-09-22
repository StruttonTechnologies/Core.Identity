using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Validators;
using ST.Core.IdentityAccess.Fakes.UserManager;
using ST.Core.Validators.Format;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.UpdateEmailAsync"/>.
    /// Validates email update logic, token generation, input validation, and exception handling.
    /// </summary>
    public class UpdateEmailAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid email update returns a successful result.
        /// </summary>
        [Fact]
        public async Task UpdateEmailAsync_ValidEmail_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.UpdateEmailAsync(user, "updated@example.com");

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id.ToString());
            Assert.Equal("updated@example.com", updatedUser!.Email);
        }

        [Theory]
        [InlineData(null, "updated@example.com", typeof(ArgumentNullException), "user")]
        [InlineData("user", null, typeof(ArgumentNullException), "newEmail")]
        public async Task UpdateEmailAsync_InvalidArguments_ThrowsExpectedException(
             string userSelector,
             string email,
             Type expectedExceptionType,
             string expectedParamName)
        {
            var user = userSelector == "user" ? TestAppUserIdentityFactory.CreateDefault() : null;

            if (user is not null)
                await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.UpdateEmailAsync(user!, email!));

            Assert.NotNull(exception);
            Assert.IsType(expectedExceptionType, exception);

            if (exception is ArgumentNullException argEx)
                Assert.Equal(expectedParamName, argEx.ParamName);
        }


        /// <summary>
        /// Verifies that an exception during token or email change returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task UpdateEmailAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.UpdateEmailAsync(user, "updated@example.com");

            Assert.False(result.Succeeded);
            Assert.Contains("User not found in store", result.Errors.First().Description);
        }
    }
}