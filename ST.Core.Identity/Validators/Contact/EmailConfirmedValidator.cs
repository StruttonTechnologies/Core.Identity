using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Validators.Contact
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public class EmailConfirmedValidator : IValidator<bool>
    {
        public ValidationResult Validate(bool input) =>
            input
                ? ValidationResult.Success()
                : ValidationResult.Failure("Email must be confirmed.");
    }

}
