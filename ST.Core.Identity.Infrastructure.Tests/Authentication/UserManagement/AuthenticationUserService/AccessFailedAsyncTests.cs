using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    /// <summary>
    /// Contains unit tests for the <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> method.
    /// </summary>
    public class AccessFailedAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> increments the failure count for a valid user.
        /// </summary>
        [Fact]
        public async Task AccessFailedAsync_ValidUser_IncrementsFailureCount()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.AccessFailedAsync(user);

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id);
            Assert.Equal(1, updatedUser!.AccessFailedCount);
        }

        /// <summary>
        /// Verifies that <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> throws <see cref="ArgumentNullException"/> when the user is null.
        /// </summary>
        [Fact]
        public async Task AccessFailedAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.AccessFailedAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that <see cref="AuthenticationUserService{TUser}.AccessFailedAsync"/> returns a failed <see cref="IdentityResult"/> when an exception occurs.
        /// </summary>
        [Fact]
        public async Task AccessFailedAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            // Simulate failure by removing user from store before calling AccessFailedAsync
            await UserManager.DeleteAsync(user);

            var result = await Service.AccessFailedAsync(user);

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}