using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class UserNameFormatValidatorTests
    {
        [Theory]
        [InlineData("ab", "UserNameTooShort")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "UserNameTooLong")]
        [InlineData("user!name", "UserNameInvalidCharacters")]
        [InlineData("valid_user-123", null)]
        public async Task ValidateAsync_UsernameFormatAndLength_ReturnsExpectedResult(string username, string? expectedErrorCode)
        {
            var user = new TestAppIdentityUser { UserName = username };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new UserNameFormatValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            if (expectedErrorCode == null)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.Contains(result.Errors, e => e.Code == expectedErrorCode);
            }
        }
    }
}