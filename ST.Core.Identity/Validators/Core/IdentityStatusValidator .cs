using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Validators.Core
{
    public enum IdentityStatus
    {
        Unknown = 0,
        Active = 1,
        Suspended = 2,
        Deleted = 3
    }

    [AutoRegister(ServiceLifetime.Singleton)]
    public class IdentityStatusValidator : IValidator<IdentityStatus>
    {
        public ValidationResult Validate(IdentityStatus input) =>
            input == IdentityStatus.Unknown
                ? ValidationResult.Failure("Identity status must be set to a valid value.")
                : ValidationResult.Success();
    }
}