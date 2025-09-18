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
    public class IdentityProviderValidator : IValidator<string>
    {
        private static readonly HashSet<string> _knownProviders = new(StringComparer.OrdinalIgnoreCase)
        {
            "Google", "Microsoft", "GitHub", "Okta", "Auth0"
        };

        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.Failure("Identity provider is required.");

            if (!_knownProviders.Contains(input))
                return ValidationResult.Failure($"Unknown identity provider: '{input}'.");

            return ValidationResult.Success();
        }
    }


}
