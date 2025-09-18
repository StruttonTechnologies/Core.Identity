using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Validators;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Validators
{
    public class AlwaysFailUserValidatorTests
    {
        [Theory]
        [InlineData("user1@example.com")]
        [InlineData("user2@example.com")]
        [InlineData("")]
        [InlineData(null)]
        public async Task ValidateAsync_AlwaysReturnsFailedResult(string? email)
        {
            var user = new TestAppIdentityUser
            {
                UserName = "testuser",
                Email = email
            };
            var userManagerMock = new Mock<UserManager<TestAppIdentityUser>>(
                Mock.Of<IUserStore<TestAppIdentityUser>>(), null, null, null, null, null, null, null, null);

            var validator = new AlwaysFailUserValidator();

            var result = await validator.ValidateAsync(userManagerMock.Object, user);

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Single(result.Errors);
            var error = Assert.Single(result.Errors);
            Assert.Equal("InvalidEmail", error.Code);
            Assert.Equal("Email format is invalid.", error.Description);
        }
    }
}