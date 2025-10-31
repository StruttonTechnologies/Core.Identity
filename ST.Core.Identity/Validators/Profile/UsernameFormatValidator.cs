using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results.Models;
using ST.Core.Validators.Results.Interfaces;
using System.Text.RegularExpressions;

namespace ST.Core.Identity.Validators.Profile
{
    /// <summary>
    /// Validates that a username matches a standard format.
    /// </summary>
    /// <remarks>
    /// Enforces the following rules:
    /// - Starts with a letter
    /// - Contains only letters, digits, underscores, or hyphens
    /// - Length between 3 and 20 characters
    /// </remarks>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class UsernameFormatValidator : IValidator<string>
    {
        private static readonly Regex _regex = new(@"^[a-zA-Z][a-zA-Z0-9_-]{2,19}$", RegexOptions.Compiled);

        /// <summary>
        /// Validates the specified username input.
        /// </summary>
        /// <param name="input">The username to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the format is valid,
        /// or failure with a descriptive message.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(
                    message: "Username is required.",
                    code: "Required",
                    field: nameof(input));
            }

            if (!_regex.IsMatch(input))
            {
                return ValidationResult.Failure(
                    message: "Username must start with a letter and contain only letters, digits, underscores, or hyphens. Length must be 3–20 characters.",
                    code: "InvalidUsernameFormat",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
