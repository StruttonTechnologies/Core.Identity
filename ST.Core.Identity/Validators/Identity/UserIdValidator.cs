using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results.Models;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Identity
{
    /// <summary>
    /// Validates that a user ID is present and properly formatted.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class UserIdValidator : IValidator<Guid>
    {
        /// <summary>
        /// Validates the specified user ID.
        /// </summary>
        /// <param name="input">The user ID to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the ID is valid,
        /// or failure if it is empty or malformed.
        /// </returns>
        public ValidationResult Validate(Guid input)
        {
            if (input == Guid.Empty)
            {
                return ValidationResult.Failure(
                    message: "User ID is required.",
                    code: "MissingUserId",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
