using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class BlacklistPhoneValidatorTests
    {
        [Theory]
        [InlineData("+10000000000")]
        [InlineData("+19999999999")]
        public async Task ValidateAsync_BlacklistedPhoneNumbers_ReturnsFailedResult(string phoneNumber)
        {
            var user = new TestAppIdentityUser { PhoneNumber = phoneNumber };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new BlacklistPhoneValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Single(result.Errors);
            var error = Assert.Single(result.Errors);
            Assert.Equal("PhoneNumberBlacklisted", error.Code);
            Assert.Equal("Phone number is not allowed.", error.Description);
        }

        [Theory]
        [InlineData("+15555555555")]
        [InlineData("+18888888888")]
        [InlineData("")]
        [InlineData(null)]
        public async Task ValidateAsync_NonBlacklistedPhoneNumbers_ReturnsSuccess(string? phoneNumber)
        {
            var user = new TestAppIdentityUser { PhoneNumber = phoneNumber };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new BlacklistPhoneValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Empty(result.Errors);
        }
    }
}