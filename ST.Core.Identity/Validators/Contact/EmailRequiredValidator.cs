using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using System.Text.RegularExpressions;

namespace ST.Core.Identity.Validators.Contact
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class EmailRequiredValidator : IValidator<string>
    {
        private static readonly Regex _regex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.Failure("Email is required.");

            if (!_regex.IsMatch(input))
                return ValidationResult.Failure("Email format is invalid.");

            return ValidationResult.Success();
        }
    }
}