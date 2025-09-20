using ST.Core.Identity.Validators.Identity;

namespace ST.Core.Identity.Tests.Validators
{
    public class EmailRequiredValidatorTests
    {
        private readonly EmailRequiredValidator _validator = new();

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("admin@domain.org")]
        public void Should_Return_Success_For_Valid_Email(string email)
        {
            var result = _validator.Validate(email);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Return_Failure_For_Missing_Email(string email)
        {
            var result = _validator.Validate(email);
            Assert.False(result.IsSuccess);
            Assert.Equal("MissingEmail", result.Code);
        }
    }
}