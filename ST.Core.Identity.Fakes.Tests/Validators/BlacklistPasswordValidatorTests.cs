using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class BlacklistPasswordValidatorTests
    {
        [Theory]
        [InlineData("password")]
        [InlineData("123456")]
        [InlineData("qwerty")]
        [InlineData("letmein")]
        [InlineData("admin")]
        [InlineData("PASSWORD")]
        [InlineData("Admin")]
        public async Task ValidateAsync_BlacklistedPasswords_ReturnsFailedResult(string blacklistedPassword)
        {
            var user = new TestAppIdentityUser { UserName = "testuser" };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new BlacklistPasswordValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user, blacklistedPassword);

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Single(result.Errors);
            var error = Assert.Single(result.Errors);
            Assert.Equal("PasswordBlacklisted", error.Code);
            Assert.Equal("Password is too common or insecure.", error.Description);
        }

        [Theory]
        [InlineData("uniquePassword!@#")]
        [InlineData("S3cureP@ssw0rd")]
        [InlineData("notblacklisted")]
        [InlineData("anotherSafePassword")]
        public async Task ValidateAsync_NonBlacklistedPasswords_ReturnsSuccess(string password)
        {
            var user = new TestAppIdentityUser { UserName = "testuser" };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new BlacklistPasswordValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user, password);

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Empty(result.Errors);
        }
    }
}