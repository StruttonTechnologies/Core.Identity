using StruttonTechnologies.Core.Identity.Validators.Security;
using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Core.Validators.Security;

/// <summary>
/// Contains test scenarios for <see cref="Identity.Validators.Security.BlacklistPasswordValidator"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class BlacklistPasswordValidatorTests
{
    private readonly BlacklistPasswordValidator _sut = new(new[] { "password", "123456" });

    /// <summary>
    /// Validates that a password not in the blacklist returns a successful validation result.
    /// </summary>
    [Fact]
    public void Validate_WithAllowedPassword_ReturnsSuccess()
    {
        ValidationResult result = _sut.Validate("Comp!exSecret123");

        Assert.True(result.IsValid);
    }

    /// <summary>
    /// Validates that a password in the blacklist returns a failed validation result with the correct error code.
    /// </summary>
    [Fact]
    public void Validate_WithBlacklistedPassword_ReturnsBlacklistedPasswordFailure()
    {
        ValidationResult result = _sut.Validate("password");

        Assert.False(result.IsValid);
        Assert.Equal("BlacklistedPassword", result.Code);
    }
}
