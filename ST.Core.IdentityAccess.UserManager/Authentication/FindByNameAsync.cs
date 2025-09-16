using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    public abstract partial class AuthenticationUserService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously finds a user by their username.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="username">The username to search for. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The user entity if found; otherwise, <c>null</c>.
        /// Returns <c>null</c> if an exception occurs.
        /// </returns>
        public virtual async Task<TUser?> FindByNameAsync(string username, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username must not be null or empty.", nameof(username));

            try
            {
                return await _userManager.FindByNameAsync(username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to find user by username {Username}", username);
                return null;
            }
        }
    }
}