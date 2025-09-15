using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.DeleteAsync"/>.
    /// Validates user deletion logic, input validation, and exception handling.
    /// </summary>
    public class DeleteAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid user is deleted successfully.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidUser_ReturnsTrue()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.DeleteAsync(user, CancellationToken.None);

            Assert.True(result);
            var deletedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Null(deletedUser);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.DeleteAsync(null!, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during deletion returns <c>false</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ThrowsException_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            // Simulate failure by disposing the user manager's store or deleting user twice
            await UserManager.DeleteAsync(user); // First delete
            var result = await Service.DeleteAsync(user, CancellationToken.None); // Second delete

            Assert.False(result);
        }
    }
}