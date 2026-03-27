using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Security
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Security.StrictPasswordValidator"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StrictPasswordValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Security.StrictPasswordValidator _sut = new();

        [Fact]
        public void Validate_WithStrongPassword_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate("StrongPass1!");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithWeakPassword_ReturnsFailure()
        {
            ValidationResult result = _sut.Validate("password");

            Assert.False(result.IsValid);
        }
    }
}
