using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Models;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.GenerateUserTokenAsync"/>.
    /// Validates token generation for a specific purpose, input validation, and exception handling.
    /// </summary>
    public class GenerateUserTokenAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid user, provider, and purpose return a non-empty token.
        /// </summary>
        [Fact]
        public async Task GenerateUserTokenAsync_ValidInputs_ReturnsToken()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            UserManager.RegisterTokenProvider("CustomProvider", new EmailTokenProvider<TestAppIdentityUser>());

            var token = await Service.GenerateUserTokenAsync(user, "CustomProvider", "CustomPurpose");

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task GenerateUserTokenAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() =>
                Service.GenerateUserTokenAsync(null!, "CustomProvider", "CustomPurpose"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that null or empty token provider throws <see cref="ArgumentException"/>.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GenerateUserTokenAsync_InvalidProvider_ThrowsArgumentException(string? provider)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() =>
                Service.GenerateUserTokenAsync(user, provider!, "CustomPurpose"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that null or empty purpose throws <see cref="ArgumentException"/>.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GenerateUserTokenAsync_InvalidPurpose_ThrowsArgumentException(string? purpose)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() =>
                Service.GenerateUserTokenAsync(user, "CustomProvider", purpose!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during token generation returns <c>null</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task GenerateUserTokenAsync_ThrowsException_ReturnsNull()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            UserManager.RegisterTokenProvider("CustomProvider", new EmailTokenProvider<TestAppIdentityUser>());
            await UserManager.DeleteAsync(user); // Simulate failure

            var token = await Service.GenerateUserTokenAsync(user, "CustomProvider", "CustomPurpose");

            Assert.Null(token);
        }
    }
}