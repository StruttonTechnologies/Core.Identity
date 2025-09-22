using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Validators;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.UpdateUsernameAsync"/>.
    /// Validates username update logic, input validation, and exception handling.
    /// </summary>
    public class UpdateUsernameAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid username update returns a successful result.
        /// </summary>
        [Fact]
        public async Task UpdateUsernameAsync_ValidUsername_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);

            var result = await Service.UpdateUsernameAsync(user, "newUserName");

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id.ToString());
            Assert.Equal("newUserName", updatedUser!.UserName);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdateUsernameAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.UpdateUsernameAsync(null!, "newUserName"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null username throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task UpdateUsernameAsync_NullUsername_ThrowsArgumentNullException()
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.UpdateUsernameAsync(user, null!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}