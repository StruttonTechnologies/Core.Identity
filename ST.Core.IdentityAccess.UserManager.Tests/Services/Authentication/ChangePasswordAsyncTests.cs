using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    public class ChangePasswordAsyncTests : AuthenticationUserServiceTestBase
    {
        [Fact]
        public async Task ChangePasswordAsync_ValidInputs_ChangesPasswordSuccessfully()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var originalPassword = "OldP@ssword123";
            var newPassword = "NewP@ssword456";

            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, originalPassword);

            var result = await Service.ChangePasswordAsync(user, originalPassword, newPassword);

            Assert.True(result.Succeeded);
            var isNewPasswordValid = await UserManager.CheckPasswordAsync(user, newPassword);
            Assert.True(isNewPasswordValid);
        }

        [Theory]
        [InlineData(null, "old", "new", typeof(ArgumentNullException))]
        [InlineData("user", "", "new", typeof(ArgumentException))]
        [InlineData("user", "old", "", typeof(ArgumentException))]
        public async Task ChangePasswordAsync_InvalidArguments_ThrowsExpectedException(string? userName, string currentPassword, string newPassword, Type expectedException)
        {
            var user = userName == null ? null! : TestAppUserIdentityFactory.Create(userName);

            var exception = await Record.ExceptionAsync(() => Service.ChangePasswordAsync(user, currentPassword, newPassword));

            Assert.NotNull(exception);
            Assert.IsType(expectedException, exception);
        }

        [Fact]
        public async Task ChangePasswordAsync_ThrowsException_ReturnsFailedResult()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var originalPassword = "OldP@ssword123";
            var newPassword = "NewP@ssword456";

            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, originalPassword);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.ChangePasswordAsync(user, originalPassword, newPassword);

            Assert.False(result.Succeeded);
            Assert.Contains("Exception:", result.Errors.First().Description);
        }
    }
}
