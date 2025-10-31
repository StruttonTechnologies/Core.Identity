using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results.Interfaces;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that a session ID is present and non-empty.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class SessionIdValidator : IValidator<string>
    {
        /// <summary>
        /// Validates the specified session ID.
        /// </summary>
        /// <param name="input">The session ID to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the ID is valid,
        /// or failure if it is missing or empty.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure("Session ID is required.", "Required", "SessionId");
            }

            if (!Guid.TryParse(input, out var parsed) || parsed == Guid.Empty)
            {
                return ValidationResult.Failure("Invalid Session ID format.", "InvalidSessionId", "SessionId");
            }

            return ValidationResult.Success();
        }
    }
}
