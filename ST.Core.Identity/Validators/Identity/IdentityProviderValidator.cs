using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Identity
{
    /// <summary>
    /// Validates that an identity provider is supported and authorized.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class IdentityProviderValidator : IValidator<string>
    {
        public static readonly HashSet<string> _knownProviders = new(StringComparer.OrdinalIgnoreCase)
        {
            "Google", "Microsoft", "GitHub", "Okta", "Auth0"
        };

        /// <summary>
        /// Validates that the input provider is supported.
        /// </summary>
        /// <param name="input">The identity provider to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the provider is valid,
        /// or failure if it is unsupported.
        /// </returns>
        public IValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Identity provider is required.",
                    code: "MissingIdentityProvider",
                    field: nameof(input));
            }

            if (!_knownProviders.Contains(input))
            {
                return ValidationResultFactory.Failure(
                    message: $"Identity provider '{input}' is not supported.",
                    code: "UnsupportedIdentityProvider",
                    field: nameof(input));
            }

            return ValidationResultFactory.Success();
        }
    }
}
