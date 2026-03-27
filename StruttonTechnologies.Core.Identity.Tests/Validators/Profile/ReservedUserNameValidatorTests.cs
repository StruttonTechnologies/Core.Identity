using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Profile
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Profile.ReservedUserNameValidator"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ReservedUserNameValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Profile.ReservedUserNameValidator _sut = new(new[] { "admin", "root" });

        [Fact]
        public void Validate_WithAvailableUserName_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate("shawn");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithReservedUserName_ReturnsReservedUsernameFailure()
        {
            ValidationResult result = _sut.Validate("ADMIN");

            Assert.False(result.IsValid);
            Assert.Equal("ReservedUsername", result.Code);
        }
    }
}
