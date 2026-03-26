namespace StruttonTechnologies.Core.Identity.Exceptions
{
    /// <summary>
    /// Represents an exception related to username validation during identity operations.
    /// </summary>
    public class UserNameNotAvailableException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNameNotAvailableException"/> class.
        /// </summary>
        public UserNameNotAvailableException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNameNotAvailableException"/> class with a default existence message.
        /// </summary>
        /// <param name="userName">The username involved in the exception.</param>
        public UserNameNotAvailableException(string userName)
            : base($"Username '{userName}' exists.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNameNotAvailableException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UserNameNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Throws a <see cref="UserNameNotAvailableException"/> if the specified user is not null.
        /// Used to guard against duplicate usernames.
        /// </summary>
        /// <typeparam name="TUser">The user type.</typeparam>
        /// <param name="user">The user to check.</param>
        /// <param name="userName">The username involved.</param>
        public static void ThrowIfExists<TUser>(TUser? user, string userName)
            where TUser : class
        {
            if (user is not null)
            {
                throw new UserNameNotAvailableException(userName);
            }
        }

        /// <summary>
        /// Throws a <see cref="UserNameNotAvailableException"/> if the specified user is null.
        /// Used to guard against missing usernames.
        /// </summary>
        /// <typeparam name="TUser">The user type.</typeparam>
        /// <param name="user">The user to check.</param>
        /// <param name="userName">The username involved.</param>
        public static void ThrowIfNotExist<TUser>(TUser? user, string userName)
            where TUser : class
        {
            if (user is null)
            {
                throw new UserNameNotAvailableException($"Username '{userName}' does not exist.");
            }
        }

        /// <summary>
        /// Throws a <see cref="UserNameNotAvailableException"/> if the specified user is not null.
        /// Used to guard against usernames that are unavailable for registration.
        /// </summary>
        /// <typeparam name="TUser">The user type.</typeparam>
        /// <param name="user">The user to check.</param>
        /// <param name="userName">The username involved.</param>
        public static void ThrowIfNotAvailable<TUser>(TUser? user, string userName)
            where TUser : class
        {
            if (user is not null)
            {
                throw new UserNameNotAvailableException($"Username '{userName}' is not available for use.");
            }
        }
    }
}
