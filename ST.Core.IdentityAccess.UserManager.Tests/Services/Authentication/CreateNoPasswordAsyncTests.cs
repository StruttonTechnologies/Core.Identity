using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.CreateNoPasswordAsync"/>.
    /// Validates user creation without password, input validation, and exception handling.
    /// </summary>
    public class CreateNoPasswordAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid user is created successfully without a password.
        /// </summary>
        [Fact]
        public async Task CreateNoPasswordAsync_ValidUser_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();

            var result = await Service.CreateNoPasswordAsync(user);

            Assert.True(result.Succeeded);
            var createdUser = await UserManager.FindByIdAsync(user.Id);
            Assert.NotNull(createdUser);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task CreateNoPasswordAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.CreateNoPasswordAsync(null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during creation returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task CreateNoPasswordAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.CreateNoPasswordAsync(user);

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}
