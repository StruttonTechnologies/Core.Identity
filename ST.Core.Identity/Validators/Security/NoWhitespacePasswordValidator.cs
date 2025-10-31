using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results.Models;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Security
{
    /// <summary>
    /// Validates that a password does not contain any whitespace characters.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class NoWhitespacePasswordValidator : IValidator<string>
    {
        /// <summary>
        /// Validates that the input password contains no whitespace.
        /// </summary>
        /// <param name="input">The password to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if no whitespace is present,
        /// or failure if any whitespace is detected.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(
                    message: "Password is required.",
                    code: "Required",
                    field: nameof(input));
            }

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    return ValidationResult.Failure(
                        message: "Password must not contain whitespace.",
                        code: "WhitespaceDetected",
                        field: nameof(input));
                }
            }

            return ValidationResult.Success();
        }
    }
}
