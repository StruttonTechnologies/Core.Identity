using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.ConfirmEmailAsync"/>.
    /// Validates email confirmation logic, input validation, and exception handling.
    /// </summary>
    public class ConfirmEmailAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid token confirms the user's email successfully.
        /// </summary>
        [Fact]
        public async Task ConfirmEmailAsync_ValidToken_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await Service.ConfirmEmailAsync(user, token);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.True(updatedUser!.EmailConfirmed);
        }

        /// <summary>
        /// Verifies that an invalid token fails to confirm the user's email.
        /// </summary>
        [Fact]
        public async Task ConfirmEmailAsync_InvalidToken_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            var invalidToken = "invalid-token";

            var result = await Service.ConfirmEmailAsync(user, invalidToken);

            Assert.False(result.Succeeded);
        }

        /// <summary>
        /// Verifies that invalid arguments (null user or empty token) throw the expected exceptions.
        /// </summary>
        /// <param name="userName">The username to create the user with, or null to simulate a null user.</param>
        /// <param name="token">The token to confirm the email.</param>
        /// <param name="expectedException">The expected exception type.</param>
        [Theory]
        [InlineData(null, "valid-token", typeof(ArgumentNullException))]
        [InlineData("ValidUser", "", typeof(ArgumentException))]
        [InlineData("ValidUser", null, typeof(ArgumentException))]
        public async Task ConfirmEmailAsync_InvalidArguments_ThrowsExpectedException(string? userName, string? token, Type expectedException)
        {
            var user = userName == null ? null! : TestAppUserIdentityFactory.Create(userName);

            var exception = await Record.ExceptionAsync(() => Service.ConfirmEmailAsync(user, token!));

            Assert.NotNull(exception);
            Assert.IsType(expectedException, exception);
        }

        /// <summary>
        /// Verifies that an exception during confirmation returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task ConfirmEmailAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.ConfirmEmailAsync(user, token);

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}