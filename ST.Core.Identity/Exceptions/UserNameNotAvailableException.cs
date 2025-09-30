using System;

namespace ST.Core.Identity.Exceptions
{
    /// <summary>
    /// Represents an exception related to username validation during identity operations.
    /// </summary>
    public class UserException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserException"/> class with a default existence message.
        /// </summary>
        /// <param name="userName">The username involved in the exception.</param>
        public UserException(string userName)
            : base($"Username '{userName}' exists.") { }

        /// <summary>
        /// Throws a <see cref="UserException"/> if the specified user is not null.
        /// Used to guard against duplicate usernames.
        /// </summary>
        public static void ThrowIfExists<TUser>(TUser? user, string userName)
            where TUser : class
        {
            if (user is not null)
                throw new UserException(userName);
        }

        /// <summary>
        /// Throws a <see cref="UserException"/> if the specified user is null.
        /// Used to guard against missing usernames.
        /// </summary>
        public static void ThrowIfNotExist<TUser>(TUser? user, string userName)
            where TUser : class
        {
            if (user is null)
                throw new UserException($"Username '{userName}' does not exist.");
        }

        /// <summary>
        /// Throws a <see cref="UserException"/> if the specified user is not null.
        /// Used to guard against usernames that are unavailable for registration.
        /// </summary>
        public static void ThrowIfNotAvailable<TUser>(TUser? user, string userName)
            where TUser : class
        {
            if (user is not null)
                throw new UserException($"Username '{userName}' is not available for use.");
        }
    }
}