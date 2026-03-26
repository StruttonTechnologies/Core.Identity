using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Identity
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Identity.IdentityProviderValidator"/>.
    /// </summary>
    public class IdentityProviderValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Identity.IdentityProviderValidator _sut = new();

        [Theory]
        [InlineData("Google")]
        [InlineData("google")]
        [InlineData("Local")]
        public void Validate_WithKnownProvider_ReturnsSuccess(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_WithMissingProvider_ReturnsMissingIdentityProviderFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("MissingIdentityProvider", result.Code);
        }

        [Fact]
        public void Validate_WithUnknownProvider_ReturnsUnsupportedIdentityProviderFailure()
        {
            ValidationResult result = _sut.Validate("LegacySso");

            Assert.False(result.IsValid);
            Assert.Equal("UnsupportedIdentityProvider", result.Code);
        }
    }
}
