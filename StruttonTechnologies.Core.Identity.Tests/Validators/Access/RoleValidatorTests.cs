using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Access
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Access.RoleValidator"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RoleValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Access.RoleValidator _sut = new();

        [Fact]
        public void Validate_WithKnownRole_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate("Admin");

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WithMissingRole_ReturnsRequiredFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("Required", result.Code);
        }

        [Fact]
        public void Validate_WithUnknownRole_ReturnsInvalidRoleFailure()
        {
            ValidationResult result = _sut.Validate("SuperWizard");

            Assert.False(result.IsValid);
            Assert.Equal("InvalidRole", result.Code);
        }
    }
}
