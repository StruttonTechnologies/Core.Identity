using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class PhoneNumberFormatValidatorTests
    {
        [Theory]
        [InlineData("+15555555555", true)]
        [InlineData("555-555-5555", false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public async Task ValidateAsync_PhoneNumberFormat_ReturnsExpectedResult(string phoneNumber, bool isValid)
        {
            var user = new TestAppIdentityUser { PhoneNumber = phoneNumber };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new PhoneNumberFormatValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            if (isValid)
            {
                Assert.True(result.Succeeded);
                Assert.Empty(result.Errors);
            }
            else
            {
                Assert.False(result.Succeeded);
                Assert.Contains(result.Errors, e => e.Code == "PhoneNumberFormatInvalid");
            }
        }
    }
}