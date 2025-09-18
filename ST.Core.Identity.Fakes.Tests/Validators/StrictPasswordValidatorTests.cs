using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class StrictPasswordValidatorTests
    {
        [Theory]
        [InlineData("Abc123!", true)]
        [InlineData("abc", false)]
        [InlineData("123456", false)]
        [InlineData("!@#$%", false)]
        [InlineData("Abcdefgh", false)]
        public async Task ValidateAsync_StrictPassword_ReturnsExpectedResult(string password, bool isValid)
        {
            var user = new TestAppIdentityUser { UserName = "testuser" };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new StrictPasswordValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user, password);

            if (isValid)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.NotEmpty(result.Errors);
            }
        }
    }
}