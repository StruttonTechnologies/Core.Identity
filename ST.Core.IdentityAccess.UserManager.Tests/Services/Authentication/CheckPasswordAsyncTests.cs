using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    public class CheckPasswordAsyncTests : AuthenticationUserServiceTestBase
    {
        [Fact]
        public async Task CheckPasswordAsync_ValidPassword_ReturnsTrue()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var password = "ValidP@ssword123";

            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, password);

            var result = await Service.CheckPasswordAsync(user, password);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckPasswordAsync_InvalidPassword_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var correctPassword = "CorrectP@ssword123";
            var wrongPassword = "WrongP@ssword456";

            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, correctPassword);

            var result = await Service.CheckPasswordAsync(user, wrongPassword);

            Assert.False(result);
        }

        [Theory]
        [InlineData(null, "ValidPassword", typeof(ArgumentNullException))]
        [InlineData("ValidUser", "", typeof(ArgumentException))]
        [InlineData("ValidUser", null, typeof(ArgumentException))]
        public async Task CheckPasswordAsync_InvalidArguments_ThrowsExpectedException(string? userName, string? password, Type expectedException)
        {
            var user = userName == null ? null! : TestAppUserIdentityFactory.Create(userName);

            var exception = await Record.ExceptionAsync(() => Service.CheckPasswordAsync(user, password!));

            Assert.NotNull(exception);
            Assert.IsType(expectedException, exception);
        }

        [Fact]
        public async Task CheckPasswordAsync_ThrowsException_ReturnsFalse()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var password = "ValidP@ssword123";

            await UserManager.CreateAsync(user);
            await UserManager.AddPasswordAsync(user, password);
            await UserManager.DeleteAsync(user); // Simulate failure

            var result = await Service.CheckPasswordAsync(user, password);

            Assert.False(result);
        }
    }
}