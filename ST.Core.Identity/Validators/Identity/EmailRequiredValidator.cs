using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results.Interfaces;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.Validators.Identity
{
    /// <summary>
    /// Validates that an email address is present and non-empty.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class EmailRequiredValidator : IValidator<string>
    {
        /// <summary>
        /// Validates the specified email input.
        /// </summary>
        /// <param name="input">The email address to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the email is present,
        /// or failure if it is missing or empty.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(
                    message: "Email address is required.",
                    code: "MissingEmail",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}