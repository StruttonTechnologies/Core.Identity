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
    public class UsernameValidator : IValidator<string>
    {
        private static readonly Regex _regex = new(@"^[a-zA-Z0-9_.-]{3,32}$", RegexOptions.Compiled);

        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.Failure("Username is required.");

            if (!_regex.IsMatch(input))
                return ValidationResult.Failure("Username must be 3–32 characters and contain only letters, numbers, '.', '_', or '-'.");

            return ValidationResult.Success();
        }
    }
}


