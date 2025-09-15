using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.FindByLoginAsync"/>.
    /// Validates user lookup by external login, input validation, and exception handling.
    /// </summary>
    public class FindByLoginAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid login provider and key return the corresponding user.
        /// </summary>
        [Fact]
        public async Task FindByLoginAsync_ValidLogin_ReturnsUser()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddLoginAsync(user, new Microsoft.AspNetCore.Identity.UserLoginInfo("Google", "google-key-123", "Google"));

            var result = await Service.FindByLoginAsync("Google", "google-key-123");

            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
        }

        /// <summary>
        /// Verifies that an unknown login provider or key returns <c>null</c>.
        /// </summary>
        [Fact]
        public async Task FindByLoginAsync_UnknownLogin_ReturnsNull()
        {
            var result = await Service.FindByLoginAsync("Facebook", "unknown-key");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that null or empty login provider and key throw <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="loginProvider">The login provider to test.</param>
        /// <param name="providerKey">The provider key to test.</param>
        [Theory]
        [InlineData(null, "key")]
        [InlineData("", "key")]
        [InlineData("Google", null)]
        [InlineData("Google", "")]
        public async Task FindByLoginAsync_InvalidArguments_ThrowsArgumentException(string? loginProvider, string? providerKey)
        {
            var exception = await Record.ExceptionAsync(() => Service.FindByLoginAsync(loginProvider!, providerKey!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during lookup returns <c>null</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task FindByLoginAsync_ThrowsException_ReturnsNull()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.AddLoginAsync(user, new Microsoft.AspNetCore.Identity.UserLoginInfo("Google", "google-key-123", "Google"));
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.FindByLoginAsync("Google", "google-key-123");

            Assert.Null(result);
        }
    }
}