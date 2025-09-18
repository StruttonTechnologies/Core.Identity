using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Validators.Profile
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class RoleValidator : IValidator<string>
    {
        public ValidationResult Validate(string input) =>
            string.IsNullOrWhiteSpace(input)
                ? ValidationResult.Failure("Role name is required.")
                : ValidationResult.Success();
    }
}
