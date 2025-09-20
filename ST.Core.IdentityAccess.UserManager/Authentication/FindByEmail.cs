using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    public abstract partial class AuthenticationUserService<TUser, TKey>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Asynchronously finds a user by their email address.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="email">The email address to search for. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The user entity if found; otherwise, <c>null</c>.
        /// Returns <c>null</c> if an exception occurs.
        /// </returns>
        public virtual async Task<TUser?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email must not be null or empty.", nameof(email));

            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to find user by email {Email}", email);
                return null;
            }
        }
    }
}