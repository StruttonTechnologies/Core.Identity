using StruttonTechnologies.Core.Identity.Validators.Identity;

namespace StruttonTechnologies.Core.Identity.Validators.Composite
{
    /// <summary>
    /// Composite validator that ensures an authentication context is valid.
    /// Validates provider name, session ID format, and identity status.
    /// </summary>
    public class AuthenticationContextValidator : IValidator<AuthContext>
    {
        private readonly IValidator<string> _providerNameValidator;
        private readonly IValidator<string> _sessionIdValidator;
        private readonly IValidator<IdentityStatus> _identityStatusValidator;

        /// <summary>
        /// Constructs the validator with injected domain validators.
        /// </summary>
        public AuthenticationContextValidator(
            IValidator<string> providerNameValidator,
            IValidator<string> sessionIdValidator,
            IValidator<IdentityStatus> identityStatusValidator)
        {
            _providerNameValidator = providerNameValidator;
            _sessionIdValidator = sessionIdValidator;
            _identityStatusValidator = identityStatusValidator;
        }

        /// <summary>
        /// Validates the given authentication context.
        /// Returns the first failure encountered, or success if all validations pass.
        /// </summary>
        public ValidationResult Validate(AuthContext input)
        {
            ArgumentNullException.ThrowIfNull(input);

            ValidationResult providerResult = _providerNameValidator.Validate(input.Provider);
            if (!providerResult.IsValid)
            {
                return providerResult;
            }

            ValidationResult sessionResult = _sessionIdValidator.Validate(input.SessionId);
            if (!sessionResult.IsValid)
            {
                return sessionResult;
            }

            ValidationResult statusResult = _identityStatusValidator.Validate(input.Status);
            if (!statusResult.IsValid)
            {
                return statusResult;
            }

            return ValidationResult.Success();
        }
    }

    /// <summary>
    /// Represents the input payload for authentication validation.
    /// </summary>
    public record AuthContext(
        string Provider,    // The identity provider name (e.g., Google, Facebook, Local).
        string SessionId,   // The session identifier, expected to be a valid GUID string.
        IdentityStatus Status // The current identity status of the user.
    );
}
