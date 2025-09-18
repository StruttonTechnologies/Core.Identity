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
    public class CreatedAtValidator : IValidator<DateTime>
    {
        public ValidationResult Validate(DateTime input) =>
            input == default
                ? ValidationResult.Failure("Creation timestamp must be set.")
                : ValidationResult.Success();
    }
}
