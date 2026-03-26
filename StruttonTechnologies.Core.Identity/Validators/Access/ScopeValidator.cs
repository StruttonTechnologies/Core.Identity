using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that requested scopes are known and authorized.
    /// </summary>
    public class ScopeValidator : IValidator<IEnumerable<string>>
    {
        private readonly HashSet<string> _knownScopes;

        public ScopeValidator()
        {
            // Normalize known scopes to lowercase for consistent comparison
            _knownScopes = KnownScopes.All
                .Select(s => s.ToUpperInvariant())
                .ToHashSet();
        }

        /// <summary>
        /// Validates that all requested scopes are known and allowed.
        /// </summary>
        /// <param name="input">The list of scopes to validate.</param>
        /// <returns>
        /// An <see cref="ValidationResult"/> indicating success if all scopes are valid,
        /// or failure if any unknown scopes are found.
        /// </returns>
        public ValidationResult Validate(IEnumerable<string> input)
        {
            List<string>? scopesList = input?.ToList();
            if (scopesList is null || scopesList.Count == 0)
            {
                return ValidationResult.Failure(
                    message: "Scopes are required.",
                    code: "MissingScopes",
                    field: nameof(input));
            }

            foreach (string scope in scopesList)
            {
                if (!_knownScopes.Contains(scope.ToUpperInvariant()))
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
