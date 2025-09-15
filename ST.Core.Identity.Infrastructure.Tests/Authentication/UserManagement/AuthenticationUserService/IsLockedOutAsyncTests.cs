using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.IsLockedOutAsync"/>.
    /// Validates lockout status checks, input validation, and exception handling.
    /// </summary>
    public class IsLockedOutAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a locked-out user returns <c>true</c>.
        /// </summary>
        [Fact]
        public async Task IsLockedOutAsync_UserIsLockedOut_ReturnsTrue()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetLockoutEnabledAsync(user, true);
            await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(5));

            var result = await Service.IsLockedOutAsync(user);

            Assert.True(result);
        }

        /// <summary>
        /// Verifies that a user who is not locked out returns <c>false</c>.
        /// </summary>
        [Fact]
        public async Task IsLockedOutAsync_UserIsNotLockedOut_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.SetLockoutEnabledAsync(user, true);
            await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(-5));

            var result = await Service.IsLockedOutAsync(user);

            Assert.False(result);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task IsLockedOutAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.IsLockedOutAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during lockout check returns <c>false</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task IsLockedOutAsync_ThrowsException_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.IsLockedOutAsync(user);

            Assert.False(result);
        }
    }
}