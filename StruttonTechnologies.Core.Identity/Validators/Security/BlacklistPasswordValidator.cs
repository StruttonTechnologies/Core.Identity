namespace StruttonTechnologies.Core.Identity.Validators.Security
{
    /// <summary>
    /// Validates that a password is not present in a predefined blacklist.
    /// </summary>
    public class BlacklistPasswordValidator : IValidator<string>
    {
        private readonly HashSet<string> _blacklistedPasswords;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlacklistPasswordValidator"/> class.
        /// </summary>
        /// <param name="blacklistedPasswords">A collection of passwords to block.</param>
        public BlacklistPasswordValidator(IEnumerable<string> blacklistedPasswords)
        {
            _blacklistedPasswords = [.. blacklistedPasswords];
        }

        /// <summary>
        /// Validates that the input password is not blacklisted.
        /// </summary>
        /// <param name="input">The password to validate.</param>
        /// <returns>
        /// An <see cref="ValidationResult"/> indicating success if the password is allowed,
        /// or failure if it is blacklisted.
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

            if (_blacklistedPasswords.Contains(input))
            {
                return ValidationResult.Failure(
                    message: "Password is too common or insecure.",
                    code: "BlacklistedPassword",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
