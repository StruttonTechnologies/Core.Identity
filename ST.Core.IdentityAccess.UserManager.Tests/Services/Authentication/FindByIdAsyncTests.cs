using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.FindByIdAsync"/>.
    /// Validates user lookup by ID, input validation, and exception handling.
    /// </summary>
    public class FindByIdAsyncTests : AuthenticationUserServiceTestBase
    {
        /// <summary>
        /// Verifies that a valid user ID returns the corresponding user.
        /// </summary>
        [Fact]
        public async Task FindByIdAsync_ValidId_ReturnsUser()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.FindByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
        }

        /// <summary>
        /// Verifies that an unknown user ID returns <c>null</c>.
        /// </summary>
        [Fact]
        public async Task FindByIdAsync_UnknownId_ReturnsNull()
        {
            var result = await Service.FindByIdAsync(Guid.NewGuid().ToString());

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that null or empty user ID throws <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="userId">The user ID input to test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task FindByIdAsync_InvalidId_ThrowsArgumentException(string? userId)
        {
            var exception = await Record.ExceptionAsync(() => Service.FindByIdAsync(userId!));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        /// <summary>
        /// Verifies that an exception during lookup returns <c>null</c> and logs the error.
        /// </summary>
        [Fact]
        public async Task FindByIdAsync_ThrowsException_ReturnsNull()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.FindByIdAsync(user.Id);

            Assert.Null(result);
        }
    }
}