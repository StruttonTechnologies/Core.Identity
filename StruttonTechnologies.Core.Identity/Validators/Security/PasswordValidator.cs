using System.Collections.ObjectModel;

using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity
{
    /// <summary>
    /// Provides validation logic for passwords based on organizational rules.
    /// </summary>
    public static class PasswordValidator
    {
        /// <summary>
        /// The minimum required length for a valid password.
        /// </summary>
        private const int MinimumLength = 10;

        /// <summary>
        /// Provides validation for user-created passwords according to
        /// organizational standards based on NIST SP 800-63B recommendations
        /// (adapted).
        ///
        /// Current rules:
        /// 1. Password must be at least 10 characters long.
        /// 2. Password should be memorable but not easily guessable.
        /// 3. No composition rules (uppercase, numbers, symbols) are required.
        /// 4. Password must not be in the blacklist of disallowed values
        ///    (e.g., common or compromised passwords).
        /// 5. Password must not be null, empty, or whitespace.
        ///
        /// Future extensions:
        /// - Passwords could be validated against a database table of blacklisted values.
        /// - Passwords could be checked against breach data APIs (e.g. HaveIBeenPwned).
        /// - Multi-factor authentication could be enforced separately.
        /// </summary>
        public static ReadOnlyCollection<string> Validate(string password)
        {
            List<string> errors = [];

            CheckNotEmpty(password, errors);
            CheckMinimumLength(password, errors);
            CheckBlacklist(password, errors);

            return errors.AsReadOnly();
        }

        /// <summary>
        /// Checks if the password is not empty or whitespace and adds an error if it is.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="errors">The list to which error messages are added.</param>
        private static void CheckNotEmpty(string password, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password cannot be empty or whitespace.");
            }
        }

        /// <summary>
        /// Checks if the password meets the minimum length requirement and adds an error if it does not.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="errors">The list to which error messages are added.</param>
        private static void CheckMinimumLength(string password, List<string> errors)
        {
            if (!string.IsNullOrEmpty(password) && password.Length < MinimumLength)
            {
                errors.Add($"Password must be at least {MinimumLength} characters long.");
            }
        }

        /// <summary>
        /// Checks if the password is present in the blacklist and adds an error if it is.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="errors">The list to which error messages are added.</param>
        private static void CheckBlacklist(string password, List<string> errors)
        {
            if (!string.IsNullOrEmpty(password) && KnownPasswordBlacklist.All.Contains(password, StringComparer.OrdinalIgnoreCase))
            {
                errors.Add("Password is too common or easily guessed. Choose another.");
            }
        }
    }
}
