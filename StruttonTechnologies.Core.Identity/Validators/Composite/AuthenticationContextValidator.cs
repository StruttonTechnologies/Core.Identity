using StruttonTechnologies.Core.Identity.Models;
using StruttonTechnologies.Core.Identity.Validators.Identity;

namespace StruttonTechnologies.Core.Identity.Validators.Composite;

/// <summary>
/// Validates the authentication context required before issuing tokens or granting access.
/// </summary>
public class AuthenticationContextValidator : IValidator<AuthenticationContext>
{
    private readonly IValidator<string> _providerNameValidator;
    private readonly IValidator<string> _sessionIdValidator;
    private readonly IValidator<string> _tenantIdValidator;
    private readonly IValidator<IEnumerable<string>> _scopesValidator;
    private readonly IValidator<IdentityStatus> _identityStatusValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationContextValidator"/> class.
    /// </summary>
    /// <param name="providerNameValidator">The provider name validator.</param>
    /// <param name="sessionIdValidator">The session identifier validator.</param>
    /// <param name="tenantIdValidator">The tenant identifier validator.</param>
    /// <param name="scopesValidator">The requested scopes validator.</param>
    /// <param name="identityStatusValidator">The identity status validator.</param>
    public AuthenticationContextValidator(
        IValidator<string> providerNameValidator,
        IValidator<string> sessionIdValidator,
        IValidator<string> tenantIdValidator,
        IValidator<IEnumerable<string>> scopesValidator,
        IValidator<IdentityStatus> identityStatusValidator)
    {
        _providerNameValidator = providerNameValidator ?? throw new ArgumentNullException(nameof(providerNameValidator));
        _sessionIdValidator = sessionIdValidator ?? throw new ArgumentNullException(nameof(sessionIdValidator));
        _tenantIdValidator = tenantIdValidator ?? throw new ArgumentNullException(nameof(tenantIdValidator));
        _scopesValidator = scopesValidator ?? throw new ArgumentNullException(nameof(scopesValidator));
        _identityStatusValidator = identityStatusValidator ?? throw new ArgumentNullException(nameof(identityStatusValidator));
    }

    /// <summary>
    /// Validates the supplied authentication context.
    /// </summary>
    /// <param name="input">The authentication context to validate.</param>
    /// <returns>A validation result indicating success or the first failure encountered.</returns>
    public ValidationResult Validate(AuthenticationContext input)
    {
        ArgumentNullException.ThrowIfNull(input);

        ValidationResult providerResult = _providerNameValidator.Validate(input.ProviderName);
        if (!providerResult.IsValid)
        {
            return providerResult;
        }

        ValidationResult sessionResult = _sessionIdValidator.Validate(input.SessionId);
        if (!sessionResult.IsValid)
        {
            return sessionResult;
        }

        ValidationResult tenantResult = _tenantIdValidator.Validate(input.TenantId);
        if (!tenantResult.IsValid)
        {
            return tenantResult;
        }

        ValidationResult scopesResult = _scopesValidator.Validate(input.Scopes);
        if (!scopesResult.IsValid)
        {
            return scopesResult;
        }

        ValidationResult statusResult = _identityStatusValidator.Validate(input.Status);
        if (!statusResult.IsValid)
        {
            return statusResult;
        }

        return ValidationResult.Success();
    }
}
