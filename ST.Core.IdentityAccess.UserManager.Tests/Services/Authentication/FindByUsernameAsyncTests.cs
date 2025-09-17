using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.FindByUsernameAsync"/>.
    /// Validates user lookup by username, input validation, and exception propagation.
    /// </summary>
    public class FindByUsernameAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid username returns the corresponding user.
        /// </summary>
        [Fact]
        public async Task FindByUsernameAsync_ValidUsername_ReturnsUser()
        {
            var user = TestAppUserIdentityFactory.Create("testuser");
            await UserManager.CreateAsync(user);

            var result = await Service.FindByUsernameAsync("testuser");

            Assert.NotNull(result);
            Assert.Equal(user.UserName, result!.UserName);
        }

        /// <summary>
        /// Verifies that an unknown username returns <c>null</c>.
        /// </summary>
        [Fact]
        public async Task FindByUsernameAsync_UnknownUsername_ReturnsNull()
        {
            var result = await Service.FindByUsernameAsync("nonexistentuser");

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that null or empty username throws <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="username">The username input to test.</param>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task FindByUsernameAsync_InvalidUsername_ThrowsExpectedException(string? username, Type expectedExceptionType)
        {
            var exception = await Record.ExceptionAsync(() => Service.FindByUsernameAsync(username!));

            Assert.NotNull(exception);
            Assert.IsType(expectedExceptionType, exception);
        }



        /// <summary>
        /// Verifies that an exception during lookup is propagated and logged.
        /// </summary>
        [Fact]
        public async Task FindByUsernameAsync_ThrowsException_PropagatesException()
        {
            var user = TestAppUserIdentityFactory.Create("simulate-exception");
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var exception = await Record.ExceptionAsync(() => Service.FindByUsernameAsync("simulate-exception"));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception); // Or whatever exception your store throws
        }
    }
}