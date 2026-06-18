using StruttonTechnologies.Core.Identity.Data;
using StruttonTechnologies.Core.Identity.Models;
using StruttonTechnologies.Core.Identity.Validators.Composite;
using StruttonTechnologies.Core.Identity.Validators.Identity;
using StruttonTechnologies.Core.ToolKit.Validation.Abstractions;
using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.EntityFramework.Validators;

[ExcludeFromCodeCoverage]
public class AuthenticationContextValidatorTests
{
    private readonly AuthenticationContextValidator _validator;

    public AuthenticationContextValidatorTests()
    {
        Mock<IValidator<string>> providerValidator = CreateStringValidator(ValidationResult.Success());
        Mock<IValidator<string>> sessionValidator = CreateStringValidator(ValidationResult.Success());
        Mock<IValidator<string>> tenantValidator = CreateStringValidator(ValidationResult.Success());
        Mock<IValidator<IEnumerable<string>>> scopesValidator = CreateScopesValidator(ValidationResult.Success());
        Mock<IValidator<IdentityStatus>> statusValidator = CreateStatusValidator(ValidationResult.Success());

        _validator = new AuthenticationContextValidator(
            providerValidator.Object,
            sessionValidator.Object,
            tenantValidator.Object,
            scopesValidator.Object,
            statusValidator.Object);
    }

    public static IEnumerable<object[]> ValidContexts =>
        new[]
        {
            new object[]
            {
                CreateContext(
                    KnownIdentityProviders.Local,
                    IdentityStatus.Active),
            },
            new object[]
            {
                CreateContext(
                    KnownIdentityProviders.Google,
                    IdentityStatus.Pending),
            },
        };

    [Theory]
    [MemberData(nameof(ValidContexts))]
    public void Validate_Should_ReturnSuccess_WhenContextIsValid(AuthenticationContext context)
    {
        ValidationResult result = _validator.Validate(context);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Should_ThrowArgumentNullException_WhenContextIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _validator.Validate(null!));
    }

    [Fact]
    public void Validate_Should_ReturnProviderFailure_WhenProviderIsInvalid()
    {
        ValidationResult expectedFailure = ValidationResult.Failure(
            "Invalid provider",
            "InvalidProvider",
            "ProviderName");

        Mock<IValidator<string>> providerValidator = CreateStringValidator(expectedFailure);

        AuthenticationContextValidator validator = CreateValidator(providerValidator: providerValidator);

        AuthenticationContext context = CreateContext(
            "InvalidProvider",
            IdentityStatus.Active);

        ValidationResult result = validator.Validate(context);

        Assert.False(result.IsValid);
        Assert.Equal("InvalidProvider", result.Code);
    }

    [Fact]
    public void Validate_Should_ReturnSessionFailure_WhenSessionIdIsInvalid()
    {
        ValidationResult expectedFailure = ValidationResult.Failure(
            "Invalid session",
            "InvalidSessionId",
            "SessionId");

        Mock<IValidator<string>> sessionValidator = CreateStringValidator(expectedFailure);

        AuthenticationContextValidator validator = CreateValidator(sessionValidator: sessionValidator);

        AuthenticationContext context = CreateContext(
            KnownIdentityProviders.Local,
            IdentityStatus.Active);

        ValidationResult result = validator.Validate(context);

        Assert.False(result.IsValid);
        Assert.Equal("InvalidSessionId", result.Code);
    }

    [Fact]
    public void Validate_Should_ReturnTenantFailure_WhenTenantIdIsInvalid()
    {
        ValidationResult expectedFailure = ValidationResult.Failure(
            "Invalid tenant",
            "InvalidTenantId",
            "TenantId");

        Mock<IValidator<string>> tenantValidator = CreateStringValidator(expectedFailure);

        AuthenticationContextValidator validator = CreateValidator(tenantValidator: tenantValidator);

        AuthenticationContext context = CreateContext(
            KnownIdentityProviders.Local,
            IdentityStatus.Active);

        ValidationResult result = validator.Validate(context);

        Assert.False(result.IsValid);
        Assert.Equal("InvalidTenantId", result.Code);
    }

    [Fact]
    public void Validate_Should_ReturnScopesFailure_WhenScopesAreInvalid()
    {
        ValidationResult expectedFailure = ValidationResult.Failure(
            "Invalid scopes",
            "InvalidScope",
            "Scopes");

        Mock<IValidator<IEnumerable<string>>> scopesValidator = CreateScopesValidator(expectedFailure);

        AuthenticationContextValidator validator = CreateValidator(scopesValidator: scopesValidator);

        AuthenticationContext context = CreateContext(
            KnownIdentityProviders.Local,
            IdentityStatus.Active);

        ValidationResult result = validator.Validate(context);

        Assert.False(result.IsValid);
        Assert.Equal("InvalidScope", result.Code);
    }

    [Fact]
    public void Validate_Should_ReturnStatusFailure_WhenStatusIsInvalid()
    {
        ValidationResult expectedFailure = ValidationResult.Failure(
            "Invalid status",
            "InvalidIdentityStatus",
            "Status");

        Mock<IValidator<IdentityStatus>> statusValidator = CreateStatusValidator(expectedFailure);

        AuthenticationContextValidator validator = CreateValidator(statusValidator: statusValidator);

        AuthenticationContext context = CreateContext(
            KnownIdentityProviders.Local,
            IdentityStatus.Locked);

        ValidationResult result = validator.Validate(context);

        Assert.False(result.IsValid);
        Assert.Equal("InvalidIdentityStatus", result.Code);
    }

    private static AuthenticationContext CreateContext(
        string providerName,
        IdentityStatus status)
    {
        return new AuthenticationContext
        {
            ProviderName = providerName,
            SessionId = Guid.NewGuid().ToString(),
            TenantId = Guid.NewGuid().ToString(),
            Scopes = new[] { KnownScopes.OpenId, KnownScopes.Profile },
            Status = status,
        };
    }

    private static AuthenticationContextValidator CreateValidator(
        Mock<IValidator<string>>? providerValidator = null,
        Mock<IValidator<string>>? sessionValidator = null,
        Mock<IValidator<string>>? tenantValidator = null,
        Mock<IValidator<IEnumerable<string>>>? scopesValidator = null,
        Mock<IValidator<IdentityStatus>>? statusValidator = null)
    {
        return new AuthenticationContextValidator(
            (providerValidator ?? CreateStringValidator(ValidationResult.Success())).Object,
            (sessionValidator ?? CreateStringValidator(ValidationResult.Success())).Object,
            (tenantValidator ?? CreateStringValidator(ValidationResult.Success())).Object,
            (scopesValidator ?? CreateScopesValidator(ValidationResult.Success())).Object,
            (statusValidator ?? CreateStatusValidator(ValidationResult.Success())).Object);
    }

    private static Mock<IValidator<string>> CreateStringValidator(ValidationResult result)
    {
        Mock<IValidator<string>> validator = new();
        validator
            .Setup(v => v.Validate(It.IsAny<string>()))
            .Returns(result);

        return validator;
    }

    private static Mock<IValidator<IEnumerable<string>>> CreateScopesValidator(ValidationResult result)
    {
        Mock<IValidator<IEnumerable<string>>> validator = new();
        validator
            .Setup(v => v.Validate(It.IsAny<IEnumerable<string>>()))
            .Returns(result);

        return validator;
    }

    private static Mock<IValidator<IdentityStatus>> CreateStatusValidator(ValidationResult result)
    {
        Mock<IValidator<IdentityStatus>> validator = new();
        validator
            .Setup(v => v.Validate(It.IsAny<IdentityStatus>()))
            .Returns(result);

        return validator;
    }
}
