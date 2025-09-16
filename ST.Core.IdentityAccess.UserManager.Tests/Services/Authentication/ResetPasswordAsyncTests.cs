using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.ResetPasswordAsync"/>.
    /// Validates password reset logic, input validation, and exception handling.
    /// </summary>
    public class ResetPasswordAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid token and new password reset the user's password successfully.
        /// </summary>
        [Fact]
        public async Task ResetPasswordAsync_ValidTokenAndPassword_ResetsPassword()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var originalPassword = "OldP@ssword123";
            var newPassword = "NewP@ssword456";

            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, originalPassword);
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);

            var result = await Service.ResetPasswordAsync(user, token, newPassword);

            Assert.True(result.Succeeded);
            var isNewPasswordValid = await UserManager.CheckPasswordAsync(user, newPassword);
            Assert.True(isNewPasswordValid);
        }

        /// <summary>
        /// Verifies that passing null or empty arguments throws the expected exceptions.
        /// </summary>
        [Theory]
        [InlineData(null, "token", "newPassword", typeof(ArgumentNullException))]
        [InlineData("user", "", "newPassword", typeof(ArgumentException))]
        [InlineData("user", "token", "", typeof(ArgumentException))]
        public async Task ResetPasswordAsync_InvalidArguments_ThrowsExpectedException(string? userName, string token, string newPassword, Type expectedException)
        {
            var user = userName == null ? null! : TestAppUserIdentityFactory.Create(userName);

            var exception = await Record.ExceptionAsync(() => Service.ResetPasswordAsync(user, token, newPassword));

            Assert.NotNull(exception);
            Assert.IsType(expectedException, exception);
        }

        /// <summary>
        /// Verifies that an exception during password reset returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task ResetPasswordAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, "OldP@ssword123");
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.ResetPasswordAsync(user, token, "NewP@ssword456");

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}