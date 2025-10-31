using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Validators.Identity;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.Validators.Composite
{
    /// <summary>
    /// Composite validator that ensures an authentication context is valid.
    /// Validates provider name, session ID format, and identity status.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
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
            var providerResult = _providerNameValidator.Validate(input.Provider);
            if (!providerResult.IsValid)
                return providerResult;

            var sessionResult = _sessionIdValidator.Validate(input.SessionId);
            if (!sessionResult.IsValid)
                return sessionResult;

            var statusResult = _identityStatusValidator.Validate(input.Status);
            if (!statusResult.IsValid)
                return statusResult;

            return ValidationResult.Success();
        }


    }

    /// <summary>
    /// Represents the input payload for authentication validation.
    /// </summary>
    public record AuthContext(
        /// <summary> The identity provider name (e.g., Google, Facebook, Local). </summary>
        string Provider,

        /// <summary> The session identifier, expected to be a valid GUID string. </summary>
        string SessionId,

        /// <summary> The current identity status of the user. </summary>
        IdentityStatus Status
    );
}