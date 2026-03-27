using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Security
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Security.BlacklistPasswordValidator"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BlacklistPasswordValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Security.BlacklistPasswordValidator _sut = new(new[] { "password", "123456" });

        [Fact]
        public void Validate_WithAllowedPassword_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate("Comp!exSecret123");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithBlacklistedPassword_ReturnsBlacklistedPasswordFailure()
        {
            ValidationResult result = _sut.Validate("password");

            Assert.False(result.IsValid);
            Assert.Equal("BlacklistedPassword", result.Code);
        }
    }
}
