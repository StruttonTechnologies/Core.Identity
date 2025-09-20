using ST.Core.Identity.Validators.Access;
using Xunit;

namespace ST.Core.Identity.Tests.Validators.Access
{
    public class RoleValidatorTests
    {
        private readonly RoleValidator _validator = new();

        [Theory]
        [InlineData("Admin")]
        [InlineData("user")]
        [InlineData("SUPPORT")]
        public void Should_Return_Success_For_Allowed_Roles(string role)
        {
            var result = _validator.Validate(role);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("UnknownRole")]
        public void Should_Return_Failure_For_Invalid_Or_Missing_Roles(string role)
        {
            var result = _validator.Validate(role);
            Assert.False(result.IsSuccess);
        }
    }
}