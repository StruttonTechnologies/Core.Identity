using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class RegexPasswordValidatorTests
    {
        [Theory]
        [InlineData("Abc123!", true)]
        [InlineData("abc", false)]
        [InlineData("123456", false)]
        [InlineData("!@#$%", false)]
        public async Task ValidateAsync_RegexPassword_ReturnsExpectedResult(string password, bool isValid)
        {
            var user = new TestAppIdentityUser { UserName = "testuser" };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            // Example pattern: at least one letter, one digit, one special char, min 6 chars
            var validator = new RegexPasswordValidator("^[A-Z].*", "Password must start with an uppercase letter.");

            var result = await validator.ValidateAsync(userManagerMock.Object, user, password);

            if (isValid)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.Contains(result.Errors, e => e.Code == "PasswordRegexFailed");
            }
        }
    }
}