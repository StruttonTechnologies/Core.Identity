using ST.Core.Validators;
using ST.Core.Validators.Results.Models;
using ST.Core.Validators.Results.Interfaces;

public class ScopeValidator : IValidator<IEnumerable<string>>
{
    private readonly HashSet<string> _allowedScopes;

    public ScopeValidator(IEnumerable<string> allowedScopes)
    {
        _allowedScopes = new HashSet<string>(allowedScopes, StringComparer.OrdinalIgnoreCase);
    }

    public ValidationResult Validate(IEnumerable<string> input)
    {
        if (input is null)
        {
            return ValidationResult.Failure("Scopes are required.", "MissingScopes", nameof(input));
        }

        foreach (var scope in input)
        {
            if (!_allowedScopes.Contains(scope))
            {
                return ValidationResult.Failure($"Scope '{scope}' is not recognized or authorized.", "InvalidScope", nameof(input));
            }
        }

        return ValidationResult.Success();
    }
}