using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

public class RoleValidator : IValidator<string>
{
    private readonly HashSet<string> _allowedRoles;

    public RoleValidator(IEnumerable<string> allowedRoles)
    {
        _allowedRoles = new HashSet<string>(allowedRoles, StringComparer.OrdinalIgnoreCase);
    }

    public IValidationResult Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResultFactory.Failure("Role is required.", "Required", nameof(input));
        }

        if (!_allowedRoles.Contains(input))
        {
            return ValidationResultFactory.Failure($"Role '{input}' is not recognized or authorized.", "InvalidRole", nameof(input));
        }

        return ValidationResultFactory.Success();
    }
}