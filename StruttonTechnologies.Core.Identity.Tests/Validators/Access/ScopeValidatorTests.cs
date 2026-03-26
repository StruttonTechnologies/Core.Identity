using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Access
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Access.ScopeValidator"/>.
    /// </summary>
    public class ScopeValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Access.ScopeValidator _sut = new();
        private static readonly string[] input = new[] { "openid", "api.read" };

        [Fact]
        public void Validate_WithKnownScopes_ReturnsSuccess()
        {
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
            ValidationResult result = _sut.Validate(new[] { "openid", "unknown.scope" });

            Assert.False(result.IsValid);
            Assert.Equal("InvalidScope", result.Code);
        }
    }
}
