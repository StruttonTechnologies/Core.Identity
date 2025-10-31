using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Data;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results.Interfaces;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that requested scopes are known and authorized.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class ScopeValidator : IValidator<IEnumerable<string>>
    {
        private readonly HashSet<string> _knownScopes;

        public ScopeValidator()
        {
            // Normalize known scopes to lowercase for consistent comparison
            _knownScopes = KnownScopes.All
                .Select(s => s.ToLowerInvariant())
                .ToHashSet();
        }

        /// <summary>
        /// Validates that all requested scopes are known and allowed.
        /// </summary>
        /// <param name="input">The list of scopes to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if all scopes are valid,
        /// or failure if any unknown scopes are found.
        /// </returns>
        public ValidationResult Validate(IEnumerable<string> input)
        {
            if (input is null || !input.Any())
            {
                return ValidationResult.Failure(
                    message: "Scopes are required.",
                    code: "MissingScopes",
                    field: nameof(input));
            }

            foreach (var scope in input)
            {
                if (!_knownScopes.Contains(scope.ToLowerInvariant()))
                {
                    return ValidationResult.Failure(
                        message: $"Scope '{scope}' is not recognized or authorized.",
                        code: "InvalidScope",
                        field: nameof(input));
                }
            }

            return ValidationResult.Success();
        }
    }
}