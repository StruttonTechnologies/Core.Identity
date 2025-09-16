using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Models;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.GenerateTwoFactorTokenAsync"/>.
    /// Validates token generation logic, input validation, and exception handling.
    /// </summary>
    public class GenerateTwoFactorTokenAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid user and provider return a non-empty two-factor token.
        /// </summary>
        [Fact]
        public async Task GenerateTwoFactorTokenAsync_ValidUserAndProvider_ReturnsToken()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            UserManager.RegisterTokenProvider("TestProvider", new EmailTokenProvider<TestAppIdentityUser>());

            var token = await Service.GenerateTwoFactorTokenAsync(user, "TestProvider");

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task GenerateTwoFactorTokenAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.GenerateTwoFactorTokenAsync(null!, "TestProvider"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null or empty provider throws <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="provider">The token provider input to test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GenerateTwoFactorTokenAsync_InvalidProvider_ThrowsArgumentException(string? provider)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.GenerateTwoFactorTokenAsync(user, provider!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during token generation returns <c>null</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task GenerateTwoFactorTokenAsync_ThrowsException_ReturnsNull()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            UserManager.RegisterTokenProvider("TestProvider", new EmailTokenProvider<TestAppIdentityUser>());
            await UserManager.DeleteAsync(user); // Simulate failure

            var token = await Service.GenerateTwoFactorTokenAsync(user, "TestProvider");

            Assert.Null(token);
        }
    }
}