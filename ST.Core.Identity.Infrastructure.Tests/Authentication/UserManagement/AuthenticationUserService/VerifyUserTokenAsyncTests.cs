using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.VerifyUserTokenAsync"/>.
    /// Validates token verification logic for a specific purpose, input validation, and exception handling.
    /// </summary>
    public class VerifyUserTokenAsyncTests : AuthenticationUserServiceTestBase
    {
        private const string Purpose = "CustomPurpose";
        private static readonly string Provider = TokenOptions.DefaultProvider;

        /// <summary>
        /// Verifies that a valid token for a given purpose returns <c>true</c>.
        /// </summary>
        [Fact]
        public async Task ValidToken_ReturnsTrue()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var token = await UserManager.GenerateUserTokenAsync(user, Provider, Purpose);
            var result = await Service.VerifyUserTokenAsync(user, Provider, Purpose, token);

            Assert.True(result);
        }

        /// <summary>
        /// Verifies that an invalid token returns <c>false</c>.
        /// </summary>
        [Fact]
        public async Task InvalidToken_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.VerifyUserTokenAsync(user, Provider, Purpose, "invalid-token");

            Assert.False(result);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyUserTokenAsync(null!, Provider, Purpose, "token"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that null or empty token provider throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task InvalidProvider_ThrowsArgumentNullException(string? provider)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyUserTokenAsync(user, provider!, Purpose, "token"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that null or empty purpose throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task InvalidPurpose_ThrowsArgumentNullException(string? purpose)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyUserTokenAsync(user, Provider, purpose!, "token"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that null or empty token throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task InvalidToken_ThrowsArgumentNullException(string? token)
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyUserTokenAsync(user, Provider, Purpose, token!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during verification returns <c>false</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task VerificationThrows_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.VerifyUserTokenAsync(user, Provider, Purpose, "any-token");

            Assert.False(result);
        }
    }
}