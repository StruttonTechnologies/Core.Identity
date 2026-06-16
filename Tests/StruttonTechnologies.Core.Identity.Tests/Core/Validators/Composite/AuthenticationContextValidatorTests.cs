using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Extensions;
using StruttonTechnologies.Core.Identity.Models;
using StruttonTechnologies.Core.Identity.Validators.Access;
using StruttonTechnologies.Core.Identity.Validators.Composite;
using StruttonTechnologies.Core.Identity.Validators.Identity;
using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Core.Validators.Composite;

/// <summary>
/// Contains test scenarios for <see cref="AuthenticationContextValidator"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class AuthenticationContextValidatorTests
{
    private readonly AuthenticationContextValidator _sut = new(
        new IdentityProviderValidator(),
        new SessionIdValidator(),
        new IdentityStatusValidator());

    [Fact]
    public void Validate_WithValidContext_ReturnsSuccess()
    {
        AuthenticationContext context = new()
        {
            ProviderName = "Local",
            SessionId = Guid.NewGuid().ToString(),
            TenantId = Guid.NewGuid().ToString(),
            Status = IdentityStatus.Active,
        };

        ValidationResult result = _sut.Validate(context.ToAuthContext());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithNullContext_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.Validate(null!));
    }

    [Fact]
    public void Validate_WithInvalidProvider_ReturnsFirstFailure()
    {
        AuthContext context = new AuthContext("Unknown", Guid.NewGuid().ToString(), IdentityStatus.Active);

        ValidationResult result = _sut.Validate(context);

        Assert.False(result.IsValid);
        Assert.Equal("UnsupportedIdentityProvider", result.Code);
    }
}
