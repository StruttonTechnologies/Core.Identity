using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class ReservedUserNameValidatorTests
    {
        [Theory]
        [InlineData("admin", false)]
        [InlineData("root", false)]
        [InlineData("system", false)]
        [InlineData("user", true)]
        [InlineData("guest", true)]
        public async Task ValidateAsync_ReservedUserNames_ReturnsExpectedResult(string username, bool isAllowed)
        {
            var user = new TestAppIdentityUser { UserName = username };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new ReservedUserNameValidator(
             new[] { "admin", "root", "system" },
               "User name is reserved."
           );

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            if (isAllowed)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.Contains(result.Errors, e => e.Code == "UserNameReserved");
            }
        }
    }
}