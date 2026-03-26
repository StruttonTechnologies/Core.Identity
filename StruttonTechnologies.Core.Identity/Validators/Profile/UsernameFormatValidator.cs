using System.Text.RegularExpressions;

using StruttonTechnologies.Core.ToolKit.GuardKit;

namespace StruttonTechnologies.Core.Identity.Validators.Profile
{
    /// <summary>
    /// Validates that a username matches a standard format.
    /// </summary>
    /// <remarks>
    /// Enforces the following rules:
    /// - Starts with a letter
    /// - Contains only letters, digits, underscores, or hyphens
    /// - Length between 3 and 20 characters.
    /// </remarks>
    public partial class UsernameFormatValidator : IValidator<string>
    {
        private static readonly Regex UsernameRegexPattern = UsernameRegex();

        /// <summary>
        /// Validates the specified username input.
        /// </summary>
        /// <param name="input">The username to validate.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating success if the format is valid,
        /// or failure with a descriptive message.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            return Guard.IsNullOrWhiteSpace(input)
                 .ReturnValidation(
                    message: "Username is required.",
                    code: "Required",
                    field: nameof(input),
                    () => ValidateFormat(input));
        }

        /// <summary>
        /// Validates the username format after required-value checks have passed.
        /// </summary>
        /// <param name="input">The username to validate.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating whether the username format is valid.
        /// </returns>
        private static ValidationResult ValidateFormat(string input)
        {
            if (!UsernameRegexPattern.IsMatch(input))
            {
                return ValidationResult.Failure(
                    message: "Username must start with a letter and contain only letters, digits, underscores, or hyphens. Length must be 3–20 characters.",
                    code: "InvalidUsernameFormat",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }

        [GeneratedRegex(@"^[a-zA-Z][a-zA-Z0-9_-]{2,19}$", RegexOptions.Compiled)]
        private static partial Regex UsernameRegex();
    }
}
