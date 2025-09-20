using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Data;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that requested scopes are known and authorized.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class ScopeValidator : IValidator<IEnumerable<string>>
    {
        /// <summary>
        /// Validates that all requested scopes are known and allowed.
        /// </summary>
        /// <param name="input">The list of scopes to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if all scopes are valid,
        /// or failure if any unknown scopes are found.
        /// </returns>
        public IValidationResult Validate(IEnumerable<string> input)
        {
            if (input is null)
            {
                return ValidationResultFactory.Failure(
                    message: "Scopes are required.",
                    code: "MissingScopes",
                    field: nameof(input));
            }

            foreach (var scope in input)
            {
                if (!IdentitySeed.AllowedScopes.Contains(scope))
                {
                    return ValidationResultFactory.Failure(
                        message: $"Scope '{scope}' is not recognized or authorized.",
                        code: "InvalidScope",
                        field: nameof(input));
                }
            }

            return ValidationResultFactory.Success();
        }
    }
}