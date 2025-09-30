using ST.Core.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ST.Core.Identity.Exceptions
{
    /// <summary>
    /// Exception thrown when one or more user validation rules fail.
    /// Carries a list of validation errors.
    /// </summary>
    public class UserValidationException : Exception
    {
        /// <summary>
        /// Gets the list of validation error messages.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Creates a new <see cref="UserValidationException"/> with a single error message.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        public UserValidationException(string message)
            : base(message)
        {
            Errors = new List<string> { message };
        }

        /// <summary>
        /// Creates a new <see cref="UserValidationException"/> with multiple error messages.
        /// </summary>
        /// <param name="errors">The collection of validation errors.</param>
        public UserValidationException(IEnumerable<string> errors)
            : base("User validation failed")
        {
            Errors = errors.ToList().AsReadOnly();
        }

        public static void ThrowIfInvalid(IEnumerable<string> errors)
        {
            if (errors != null && errors.Any())
            {
                throw new UserValidationException(errors);
            }
        }

        public static void ThrowIfUserNameInvalid(string userName)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(userName))
                errors.Add("Username cannot be empty.");
            if (IdentitySeed.ReservedUsernames.Contains(userName))
                errors.Add("This username is reserved and cannot be used.");
            ThrowIfInvalid(errors);
        }
    }
}
