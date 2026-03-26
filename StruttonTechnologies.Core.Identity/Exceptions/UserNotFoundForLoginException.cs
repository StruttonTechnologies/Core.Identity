namespace StruttonTechnologies.Core.Identity.Exceptions
{
    /// <summary>
    /// Exception thrown when a user with the specified username is not found in the database during login.
    /// Indicates that registration is required for the user.
    /// </summary>
    public class UserNotFoundForLoginException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundForLoginException"/> class.
        /// </summary>
        public UserNotFoundForLoginException()
        {
            UserName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundForLoginException"/> class.
        /// This exception is triggered when a user with the given username was not found in the database,
        /// and registration is required.
        /// </summary>
        /// <param name="userName">The username that was not found.</param>
        public UserNotFoundForLoginException(string userName)
            : base($"No user found for '{userName}'. Registration required.")
        {
            UserName = userName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundForLoginException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UserNotFoundForLoginException(string message, Exception innerException)
            : base(message, innerException)
        {
            UserName = string.Empty;
        }

        /// <summary>
        /// Gets the username for which the user was not found.
        /// </summary>
        public string UserName { get; }
    }
}
