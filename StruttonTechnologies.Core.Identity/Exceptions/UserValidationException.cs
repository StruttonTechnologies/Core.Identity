using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Exceptions
{
    /// <summary>
    /// Exception thrown when one or more user validation rules fail.
    /// Carries a list of validation errors.
    /// </summary>
    public class UserValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        public UserValidationException()
        {
            Errors = Array.Empty<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        public UserValidationException(string message)
            : base(message)
        {
            Errors = [message];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UserValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Errors = Array.Empty<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        /// <param name="errors">The collection of validation errors.</param>
        public UserValidationException(IEnumerable<string> errors)
            : base("User validation failed")
        {
            Errors = errors.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the list of validation error messages.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        public static void ThrowIfInvalid(IEnumerable<string> errors)
        {
            if (errors != null && errors.Any())
            {
                throw new UserValidationException(errors);
            }
        }

        public static void ThrowIfUserNameInvalid(string userName)
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add("Username cannot be empty.");
            }

            if (KnownReservedUsernames.All.Contains(userName))
            {
                errors.Add("This username is reserved and cannot be used.");
            }

            ThrowIfInvalid(errors);
        }
    }
}
