using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results.Interfaces;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.Validators.Identity
{
    /// <summary>
    /// Validates that a user's email has been confirmed.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class EmailConfirmedValidator : IValidator<bool>
    {
        /// <summary>
        /// Validates the email confirmation status.
        /// </summary>
        /// <param name="input">The email confirmation flag.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if confirmed,
        /// or failure if not confirmed.
        /// </returns>
        public ValidationResult Validate(bool input)
        {
            if (!input)
            {
                return ValidationResult.Failure(
                    message: "Email address has not been confirmed.",
                    code: "EmailNotConfirmed",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
