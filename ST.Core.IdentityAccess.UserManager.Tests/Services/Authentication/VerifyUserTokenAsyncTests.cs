using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Models;
using ST.Core.IdentityAccess.Fakes.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticationUserService{TUser}.VerifyUserTokenAsync"/>.
    /// Validates token verification logic, input validation, and exception handling.
    /// </summary>
    public class VerifyUserTokenAsyncTests : AuthenticationUserServiceTestBase
    {
        private const string Purpose = "CustomPurpose";
        private static readonly string Provider = TokenOptions.DefaultProvider;

        /// <summary>
        /// Creates and persists a default test user for verification scenarios.
        /// </summary>
        /// <returns>A newly created <see cref="TestAppIdentityUser"/> instance.</returns>
        private async Task<TestAppIdentityUser> CreateTestUserAsync()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);
            return user;
        }

        /// <summary>
        /// Verifies that a valid token for a given provider and purpose returns <c>true</c>.
        /// </summary>
        [Fact]
        public async Task ValidToken_ReturnsTrue()
        {
            var user = await CreateTestUserAsync();
            var token = await UserManager.GenerateUserTokenAsync(user, Provider, Purpose);

            var result = await Service.VerifyUserTokenAsync(user, Provider, Purpose, token);

            Assert.True(result);
        }

        /// <summary>
        /// Verifies that an invalid token returns <c>false</c>.
        /// </summary>
        [Fact]
        public async Task InvalidToken_ReturnsFalse()
        {
            var user = await CreateTestUserAsync();

            var result = await Service.VerifyUserTokenAsync(user, Provider, Purpose, "invalid-token");

            Assert.False(result);
        }

        /// <summary>
        /// Verifies that passing a <c>null</c> user throws <see cref="ArgumentNullException"/>.
        /// </summary>
        [Fact]
        public async Task NullUser_ThrowsArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyUserTokenAsync(null!, Provider, Purpose, "token"));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        /// <summary>
        /// Verifies that passing a <c>null</c> or empty provider, purpose, or token throws <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="provider">The token provider name to test.</param>
        /// <param name="purpose">The token purpose to test.</param>
        /// <param name="token">The token value to test.</param>
        /// <param name="expectedExceptionType">The expected exception type.</param>
        [Theory]
        [InlineData(null, "validPurpose", "validToken", typeof(ArgumentNullException))]
        [InlineData("", "validPurpose", "validToken", typeof(ArgumentException))]
        [InlineData("validProvider", null, "validToken", typeof(ArgumentNullException))]
        [InlineData("validProvider", "", "validToken", typeof(ArgumentException))]
        [InlineData("validProvider", "validPurpose", null, typeof(ArgumentNullException))]
        [InlineData("validProvider", "validPurpose", "", typeof(ArgumentException))]
        public async Task InvalidArguments_ThrowsExpectedException(
            string? provider, string? purpose, string? token, Type expectedExceptionType)
        {
            var user = await CreateTestUserAsync();

            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyUserTokenAsync(user, provider!, purpose!, token!));

            Assert.NotNull(exception);
            Assert.IsType(expectedExceptionType, exception);
        }

        /// <summary>
        /// Verifies that token verification returns <c>false</c> when the user has been deleted.
        /// </summary>
        [Fact]
        public async Task DeletedUser_VerificationReturnsFalse()
        {
            var user = await CreateTestUserAsync();
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.VerifyUserTokenAsync(user, Provider, Purpose, "any-token");

            Assert.False(result);
        }
    }
}