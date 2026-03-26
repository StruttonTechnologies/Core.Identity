using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Profile
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Profile.UsernameFormatValidator"/>.
    /// </summary>
    public class UsernameFormatValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Profile.UsernameFormatValidator _sut = new();

        [Theory]
        [InlineData("Shawn_01")]
        [InlineData("user-name")]
        public void Validate_WithValidUserName_ReturnsSuccess(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("1bad")]
        [InlineData("ab")]
        [InlineData("bad name")]
        public void Validate_WithInvalidUserName_ReturnsInvalidUserNameFormatFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("InvalidUsernameFormat", result.Code);
        }
    }
}
