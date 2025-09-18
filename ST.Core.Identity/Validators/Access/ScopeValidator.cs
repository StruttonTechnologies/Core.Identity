using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Validators.Access
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class ScopeValidator : IValidator<string[]>
    {
        public ValidationResult Validate(string[] input)
        {
            if (input == null || input.Length == 0)
                return ValidationResult.Failure("At least one scope is required.");

            if (input.Any(string.IsNullOrWhiteSpace))
                return ValidationResult.Failure("Scopes must not contain empty values.");

            return ValidationResult.Success();
        }
    }
}
