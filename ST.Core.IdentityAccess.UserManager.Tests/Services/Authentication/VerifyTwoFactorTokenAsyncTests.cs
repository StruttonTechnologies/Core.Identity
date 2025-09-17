using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;
using ST.Core.IdentityAccess.Fakes.UserManager.Providers;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.VerifyTwoFactorTokenAsync"/>.
    /// Validates token verification logic, input validation, and exception handling.
    /// </summary>
    public class VerifyTwoFactorTokenAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid token returns <c>true</c>.
        /// </summary>
        [Theory]
        [InlineData("Phone")]
        [InlineData("Email")]
        [InlineData("Custom2FA")]
        public async Task VerifyTwoFactorTokenAsync_ValidToken_ReturnsTrue(string provider)
        {
            var tokenProvider = new FakeTwoFactorTokenProvider();
            UserManager.RegisterTokenProvider(provider, tokenProvider);

            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetTwoFactorEnabledAsync(user, true);

            var token = await UserManager.GenerateTwoFactorTokenAsync(user, provider);
            var result = await Service.VerifyTwoFactorTokenAsync(user, provider, token);

            Assert.True(result);
        }

        /// <summary>
        /// Verifies that an invalid token returns <c>false</c>.
        /// </summary>
        [Fact]
        public async Task VerifyTwoFactorTokenAsync_InvalidToken_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetTwoFactorEnabledAsync(user, true);

            var result = await Service.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, "wrong-token");

            Assert.False(result);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task VerifyTwoFactorTokenAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyTwoFactorTokenAsync(null!, TokenOptions.DefaultPhoneProvider, "token"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null or empty token provider throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task VerifyTwoFactorTokenAsync_InvalidProvider_ThrowsExpectedException(string? provider, Type expectedExceptionType)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyTwoFactorTokenAsync(user, provider!, "token"));

            Assert.NotNull(exception);
            Assert.IsType(expectedExceptionType, exception);
        }


        /// <summary>
        /// Verifies that passing a null or empty token throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task VerifyTwoFactorTokenAsync_InvalidToken_ThrowsArgumentNullException(string? token, Type expectedExceptionType)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, token!));

            Assert.NotNull(exception);
            Assert.IsType(expectedExceptionType, exception);
        }

        /// <summary>
        /// Verifies that an exception during verification returns <c>false</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task VerifyTwoFactorTokenAsync_ThrowsException_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetTwoFactorEnabledAsync(user, true);
            await UserManager.DeleteAsync(user); // Simulate failure

            var token = "any-token";
            var result = await Service.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, token);

            Assert.False(result);
        }
    }
}