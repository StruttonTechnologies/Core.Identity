using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Interfaces;
using System;

namespace ST.Core.Identity.Validators
{
    /// <summary>
    /// Validator for user GUIDs.
    /// Ensures the GUID is not the default (empty) value.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class UserIdValidator : IValidator<Guid>
    {
        /// <summary>
        /// Validates that the provided GUID is not the default value.
        /// </summary>
        /// <param name="input">The user GUID to validate.</param>
        /// <returns>
        /// <see cref="ValidationResult"/> indicating success or failure.
        /// </returns>
        public ValidationResult Validate(Guid input)
        {
            if (input == Guid.Empty)
            {
                return ValidationResult.Failure("User ID must be a non-default GUID.");
            }

            return ValidationResult.Success();
        }
    }
}
