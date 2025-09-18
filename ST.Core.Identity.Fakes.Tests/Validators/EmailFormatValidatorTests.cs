using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class EmailFormatValidatorTests
    {
        [Theory]
        [InlineData("user@example.com", true)]
        [InlineData("user.name@domain.co", true)]
        [InlineData("invalid-email", false)]
        [InlineData("user@.com", false)]
        [InlineData("", false)]
        public async Task ValidateAsync_EmailFormat_ReturnsExpectedResult(string email, bool isValid)
        {
            var user = new TestAppIdentityUser { Email = email };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new EmailFormatValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            if (isValid)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.Contains(result.Errors, e => e.Code == "EmailFormatInvalid");
            }
        }
    }
}