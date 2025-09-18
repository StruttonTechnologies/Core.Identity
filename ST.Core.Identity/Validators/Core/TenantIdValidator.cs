using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Validators.Core
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class TenantIdValidator : IValidator<Guid>
    {
        public ValidationResult Validate(Guid input) =>
            input == Guid.Empty
                ? ValidationResult.Failure("Tenant ID must be a non-default GUID.")
                : ValidationResult.Success();
    }

}
