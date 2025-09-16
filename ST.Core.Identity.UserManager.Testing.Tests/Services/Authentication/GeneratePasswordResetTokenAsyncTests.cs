using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.GeneratePasswordResetTokenAsync"/>.
    /// Validates token generation logic, input validation, and exception handling.
    /// </summary>
    public class GeneratePasswordResetTokenAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid user returns a non-empty password reset token.
        /// </summary>
        [Fact]
        public async Task GeneratePasswordResetTokenAsync_ValidUser_ReturnsToken()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var token = await Service.GeneratePasswordResetTokenAsync(user);

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task GeneratePasswordResetTokenAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.GeneratePasswordResetTokenAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during token generation returns <c>null</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task GeneratePasswordResetTokenAsync_ThrowsException_ReturnsNull()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var token = await Service.GeneratePasswordResetTokenAsync(user);

            Assert.Null(token);
        }
    }
}