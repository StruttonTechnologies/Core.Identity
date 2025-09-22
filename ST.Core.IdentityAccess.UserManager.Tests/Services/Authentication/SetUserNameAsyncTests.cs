using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.SetUserNameAsync"/>.
    /// Validates username assignment logic, input validation, and exception handling.
    /// </summary>
    public class SetUserNameAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that setting a valid username succeeds.
        /// </summary>
        [Fact]
        public async Task SetUserNameAsync_ValidUserName_Succeeds()
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);

            var result = await Service.SetUserNameAsync(user, "newUserName");

            Assert.True(result.Succeeded);
            var updatedUser = await UserManager.FindByIdAsync(user.Id.ToString());
            Assert.Equal("newUserName", updatedUser!.UserName);
        }

        /// <summary>
        /// Verifies that passing a null user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task SetUserNameAsync_NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() => Service.SetUserNameAsync(null!, "newUserName"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a null or empty username throws the expected exception.
        /// </summary>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task SetUserNameAsync_InvalidUserName_ThrowsExpectedException(string? userName, Type expectedExceptionType)
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);

            var exception = await Record.ExceptionAsync(() => Service.SetUserNameAsync(user, userName));

            Assert.NotNull(exception);
            Assert.IsType(expectedExceptionType, exception);
        }



        /// <summary>
        /// Verifies that an exception during username assignment returns a failed result and logs the error.
        /// </summary>
        [Fact]
        public async Task SetUserNameAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.Create("originalUser");
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.SetUserNameAsync(user, "newUserName");

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}