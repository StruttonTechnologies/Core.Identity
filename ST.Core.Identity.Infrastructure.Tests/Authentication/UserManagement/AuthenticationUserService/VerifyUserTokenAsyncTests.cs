using ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup;
using ST.Core.Identity.Testing.Setup.Factories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.AuthenticationUserService
{
    public class VerifyUserTokenAsyncTests : AuthenticationUserServiceTestBase
    {
        [Fact]
        public async Task VerifyUserTokenAsync_ValidToken_ReturnsTrue()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var tokenProvider = "DefaultProvider";
            var purpose = "TestPurpose";
            var token = "ValidToken";

            // Setup UserManager mock to return true for valid token
            UserManager.SetupVerifyUserTokenAsync(user, tokenProvider, purpose, token, true);

            var result = await Service.VerifyUserTokenAsync(user, tokenProvider, purpose, token);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null, "provider", "purpose", "token", typeof(ArgumentNullException))]
        [InlineData("user", null, "purpose", "token", typeof(ArgumentNullException))]
        [InlineData("user", "provider", null, "token", typeof(ArgumentNullException))]
        [InlineData("user", "provider", "purpose", null, typeof(ArgumentNullException))]
        [InlineData("user", "", "purpose", "token", typeof(ArgumentNullException))]
        [InlineData("user", "provider", "", "token", typeof(ArgumentNullException))]
        [InlineData("user", "provider", "purpose", "", typeof(ArgumentNullException))]
        public async Task VerifyUserTokenAsync_InvalidArguments_ThrowsExpectedException(
            string? userName, string? tokenProvider, string? purpose, string? token, Type expectedException)
        {
            var user = userName == null ? null! : TestAppUserIdentityFactory.Create(userName);

            var exception = await Record.ExceptionAsync(() =>
                Service.VerifyUserTokenAsync(user, tokenProvider!, purpose!, token!));

            Assert.NotNull(exception);
            Assert.IsType(expectedException, exception);
        }

        [Fact]
        public async Task VerifyUserTokenAsync_ExceptionInUserManager_LogsErrorAndReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var tokenProvider = "DefaultProvider";
            var purpose = "TestPurpose";
            var token = "ValidToken";

            // Setup UserManager mock to throw exception
            UserManager.SetupVerifyUserTokenAsyncThrows(user, tokenProvider, purpose, token);

            var result = await Service.VerifyUserTokenAsync(user, tokenProvider, purpose, token);

            Assert.False(result);
            // Optionally verify logger was called
        }
    }
}