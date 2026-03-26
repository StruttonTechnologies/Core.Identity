using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Security
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Security.NoWhitespacePasswordValidator"/>.
    /// </summary>
    public class NoWhitespacePasswordValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Security.NoWhitespacePasswordValidator _sut = new();

        [Fact]
        public void Validate_WithoutWhitespace_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate("NoSpacesHere!");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithWhitespace_ReturnsWhitespaceDetectedFailure()
        {
            ValidationResult result = _sut.Validate("Bad Password");

            Assert.False(result.IsValid);
            Assert.Equal("WhitespaceDetected", result.Code);
        }
    }
}
