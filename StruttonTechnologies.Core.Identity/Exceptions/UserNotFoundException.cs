namespace StruttonTechnologies.Core.Identity.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when a user is expected to exist but is not found in the store.
    /// </summary>
    public class UserNotFoundException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        public UserNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public UserNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a formatted message.
        /// </summary>
        /// <param name="userId">The ID of the user that was not found.</param>
        public UserNotFoundException(object userId)
            : base($"User with ID '{userId}' was not found in the store.")
        {
        }

        /// <summary>
        /// Throws a <see cref="UserNotFoundException"/> if the specified user is null.
        /// </summary>
        /// <typeparam name="TUser">The user type.</typeparam>
        /// <param name="user">The user instance to validate.</param>
        public static void ThrowIfNull<TUser>(TUser? user)
            where TUser : class
        {
            if (user is null)
            {
                throw new UserNotFoundException("null");
            }
        }

        /// <summary>
        /// Throws a <see cref="UserNotFoundException"/> if the specified user is null,
        /// using the provided ID for context.
        /// </summary>
        /// <typeparam name="TUser">The user type.</typeparam>
        /// <param name="user">The user instance to validate.</param>
        /// <param name="userId">The ID to include in the exception message.</param>
        public static void ThrowIfNull<TUser>(TUser? user, object userId)
            where TUser : class
        {
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }
        }
    }
}
