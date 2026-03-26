using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Security
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Security.RegexPasswordValidator"/>.
    /// </summary>
    public class RegexPasswordValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Security.RegexPasswordValidator _sut = new(@"^(?=.*\d).+$", "at least one digit");

        [Fact]
        public void Validate_WithMatchingPassword_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate("Password1!");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithNonMatchingPassword_ReturnsPatternMismatchFailure()
        {
            ValidationResult result = _sut.Validate("Password!");

            Assert.False(result.IsValid);
            Assert.Equal("PatternMismatch", result.Code);
        }
    }
}
