using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    public class AddPasswordAsyncTests : AuthenticationUserServiceTestBase
    {
        [Fact]
        public async Task AddPasswordAsync_ValidUserAndPassword_ReturnsSuccess()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            var result = await Service.AddPasswordAsync(user, "SecureP@ssword123");

            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData(null, "ValidPassword", typeof(ArgumentNullException))]
        [InlineData("ValidUser", "", typeof(ArgumentException))]
        [InlineData("ValidUser", null, typeof(ArgumentException))]
        public async Task AddPasswordAsync_InvalidArguments_ThrowsExpectedException(string? userName, string? password, Type expectedException)
        {
            var user = userName == null ? null! : TestAppUserIdentityFactory.Create(userName);

            var exception = await Record.ExceptionAsync(() => Service.AddPasswordAsync(user, password!));

            Assert.NotNull(exception);
            Assert.IsType(expectedException, exception);
        }

        [Fact]
        public async Task AddPasswordAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            await UserManager.CreateAsync(user);

            // Simulate failure by deleting user before adding password
            await UserManager.DeleteAsync(user);

            var result = await Service.AddPasswordAsync(user, "SecureP@ssword123");

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}