using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Validators.Identity;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

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
        public IValidationResult Validate(AuthContext input) =>
            _providerNameValidator.Validate(input.Provider) is var providerResult && !providerResult.IsSuccess
                ? providerResult
                : _sessionIdValidator.Validate(input.SessionId) is var sessionResult && !sessionResult.IsSuccess
                    ? sessionResult
                    : _identityStatusValidator.Validate(input.Status) is var statusResult && !statusResult.IsSuccess
                        ? statusResult
                        : ValidationResultFactory.Success();
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