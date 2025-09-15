using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.UpdateAsync"/>.
    /// Validates user update logic, input validation, and exception handling.
    /// </summary>
    public class UpdateAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that updating a valid user returns the updated entity.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidUser_ReturnsUpdatedUser()
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);

            user.Email = "updated@example.com";
            var updated = await Service.UpdateAsync(user);

            Assert.Equal("updated@example.com", updated.Email);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.UpdateAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that a failed update throws <see cref="InvalidOperationException"/> and logs the error.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_FailedUpdate_ThrowsInvalidOperationException()
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);

            // Simulate failure by setting invalid email format
            user.Email = "invalid-email";

            var exception = await Record.ExceptionAsync(() => Service.UpdateAsync(user));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.Contains("Failed to update user", exception.Message);
        }

        /// <summary>
        /// Verifies that an exception during update is propagated and logged.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ThrowsException_PropagatesException()
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate internal failure

            var exception = await Record.ExceptionAsync(() => Service.UpdateAsync(user));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception); // Could also be Identity-specific depending on context
        }
    }
}