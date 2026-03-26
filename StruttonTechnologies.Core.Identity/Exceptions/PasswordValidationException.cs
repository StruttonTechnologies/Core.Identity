using System.Collections.ObjectModel;

namespace StruttonTechnologies.Core.Identity.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when password validation fails.
    /// </summary>
    public sealed class PasswordValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordValidationException"/> class.
        /// </summary>
        public PasswordValidationException()
            : base("Password validation failed.")
        {
            Errors = ReadOnlyCollection<string>.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordValidationException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PasswordValidationException(string message)
            : base(message)
        {
            Errors = ReadOnlyCollection<string>.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordValidationException"/> class
        /// with a specified error message and a reference to the inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PasswordValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Errors = ReadOnlyCollection<string>.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordValidationException"/> class
        /// with a collection of validation errors.
        /// </summary>
        /// <param name="errors">The validation errors associated with the exception.</param>
        public PasswordValidationException(IEnumerable<string> errors)
            : base("Password validation failed.")
        {
            ArgumentNullException.ThrowIfNull(errors);

            Errors = errors.ToArray().AsReadOnly();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordValidationException"/> class
        /// with a specified message and validation errors.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errors">The validation errors associated with the exception.</param>
        public PasswordValidationException(string message, IEnumerable<string> errors)
            : base(message)
        {
            ArgumentNullException.ThrowIfNull(errors);

            Errors = errors.ToArray().AsReadOnly();
        }

        /// <summary>
        /// Gets the password validation errors associated with this exception.
        /// </summary>
        public ReadOnlyCollection<string> Errors { get; }

        /// <summary>
        /// Creates a <see cref="PasswordValidationException"/> from the specified password.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>A <see cref="PasswordValidationException"/> containing the validation errors.</returns>
        public static PasswordValidationException FromPassword(string password)
        {
            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);
            return new PasswordValidationException(errors);
        }
    }
}