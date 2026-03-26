using StruttonTechnologies.Core.Identity.Validators.Access;
using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Access
{
    /// <summary>
    /// Contains test scenarios for <see cref="ScopeValidator"/>.
    /// </summary>
    public class ScopeValidatorTests
    {
        private readonly ScopeValidator _sut = new();

        [Fact]
        public void Validate_WithKnownScopes_ReturnsSuccess()
        {
            string[] input = new[] { "openid", "api.read" };

            ValidationResult result = _sut.Validate(input);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithNullScopes_ReturnsMissingScopesFailure()
        {
            ValidationResult result = _sut.Validate(null!);

            Assert.False(result.IsValid);
            Assert.Equal("MissingScopes", result.Code);
        }

        [Fact]
        public void Validate_WithUnknownScope_ReturnsInvalidScopeFailure()
        {
            string[] input = new[] { "openid", "unknown.scope" };
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("InvalidScope", result.Code);
        }
    }
}
