using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ST.Core.Identity.Validators.Profile
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class PasswordStrengthValidator : IValidator<string>
    {
        private static readonly Regex _regex = new(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$", RegexOptions.Compiled);

        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.Failure("Password is required.");

            if (!_regex.IsMatch(input))
                return ValidationResult.Failure("Password must be at least 8 characters and include uppercase, lowercase, digit, and symbol.");

            return ValidationResult.Success();
        }
    }

}
