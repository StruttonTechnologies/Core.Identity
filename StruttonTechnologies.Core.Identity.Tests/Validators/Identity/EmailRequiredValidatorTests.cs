using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Identity
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Identity.EmailRequiredValidator"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EmailRequiredValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Identity.EmailRequiredValidator _sut = new();

        [Fact]
        public void Validate_WithEmail_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate("user@example.com");

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void Validate_WithoutEmail_ReturnsMissingEmailFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("MissingEmail", result.Code);
        }
    }
}
