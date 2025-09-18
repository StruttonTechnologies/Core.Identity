using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class NoWhitespacePasswordValidatorTests
    {
        [Theory]
        [InlineData("NoSpaces123", true)]
        [InlineData("With Space", false)]
        [InlineData("Tab\tChar", false)]
        [InlineData("New\nLine", false)]
        public async Task ValidateAsync_NoWhitespacePassword_ReturnsExpectedResult(string password, bool isValid)
        {
            var user = new TestAppIdentityUser { UserName = "testuser" };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new NoWhitespacePasswordValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user, password);

            if (isValid)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.Contains(result.Errors, e => e.Code == "PasswordContainsWhitespace");
            }
        }
    }
}