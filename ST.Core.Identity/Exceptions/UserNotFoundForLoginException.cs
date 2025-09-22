using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Exceptions
{
    /// <summary>
    /// Exception thrown when a user with the specified username is not found in the database during login.
    /// Indicates that registration is required for the user.
    /// </summary>
    public class UserNotFoundForLoginException : Exception
    {
        /// <summary>
        /// Gets the username for which the user was not found.
        /// </summary>
        public string UserName { get; }

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
    }
}
