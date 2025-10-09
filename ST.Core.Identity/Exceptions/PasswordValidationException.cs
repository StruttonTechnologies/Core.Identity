using System;
using System.Collections.Generic;
using System.Linq;
using ST.Core.Identity.Data;
using ST.Core.Identity.Validators;

namespace ST.Core.Identity.Exceptions
{
    /// <summary>
    /// Exception thrown when password validation fails.
    /// Also provides static guard methods for convenient usage.
    /// </summary>
    public class PasswordValidationException : Exception
    {
        /// <summary>
        /// List of validation errors for the password.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Creates a new PasswordException with a single error message.
        /// </summary>
        private PasswordValidationException(string message)
            : base(message)
        {
            Errors = new List<string> { message };
        }

        /// <summary>
        /// Creates a new PasswordException with multiple error messages.
        /// </summary>
        private PasswordValidationException(IEnumerable<string> errors)
            : base("Password validation failed")
        {
            Errors = errors.ToList().AsReadOnly();
        }

        /// <summary>
        /// Guard method: throws if the password is invalid, aggregating all errors.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        public static void ThrowIfInvalid(string password)
        {
            var validator = new PasswordValidator();
            var errors = validator.Validate(password);

            if (errors.Any())
            {
                throw new PasswordValidationException(errors);
            }
        }

        /// <summary>
        /// Optional guard method: throws if the password is shorter than a minimum length.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <param name="minLength">Minimum required length (default 10).</param>
        public static void ThrowIfTooShort(string password, int minLength = 10)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < minLength)
            {
                throw new PasswordValidationException($"Password must be at least {minLength} characters long.");
            }
        }

        /// <summary>
        /// Optional guard method: throws if the password is in the blacklist.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        public static void ThrowIfBlacklisted(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return;

            if (KnownPasswordBlacklist.All.Contains(password, StringComparer.OrdinalIgnoreCase))
            {
                throw new PasswordValidationException(
                    "Password is too common or easily guessed. Choose another."
                );
            }
        }
    }
}
