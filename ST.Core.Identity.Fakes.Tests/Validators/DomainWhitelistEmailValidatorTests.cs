using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class DomainWhitelistEmailValidatorTests
    {
        [Theory]
        [InlineData("user@allowed.com", true)]
        [InlineData("user@another.com", false)]
        [InlineData("user@notallowed.com", false)]
        public async Task ValidateAsync_EmailDomainWhitelist_ReturnsExpectedResult(string email, bool isAllowed)
        {
            var user = new TestAppIdentityUser { Email = email };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new DomainWhitelistEmailValidator(new[] { "allowed.com" });

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            if (isAllowed)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.Contains(result.Errors, e => e.Code == "EmailDomainNotAllowed");
            }
        }
    }
}