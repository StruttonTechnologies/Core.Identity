using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

public class ScopeValidator : IValidator<IEnumerable<string>>
{
    private readonly HashSet<string> _allowedScopes;

    public ScopeValidator(IEnumerable<string> allowedScopes)
    {
        _allowedScopes = new HashSet<string>(allowedScopes, StringComparer.OrdinalIgnoreCase);
    }

    public IValidationResult Validate(IEnumerable<string> input)
    {
        if (input is null)
        {
            return ValidationResultFactory.Failure("Scopes are required.", "MissingScopes", nameof(input));
        }

        foreach (var scope in input)
        {
            if (!_allowedScopes.Contains(scope))
            {
                return ValidationResultFactory.Failure($"Scope '{scope}' is not recognized or authorized.", "InvalidScope", nameof(input));
            }
        }

        return ValidationResultFactory.Success();
    }
}