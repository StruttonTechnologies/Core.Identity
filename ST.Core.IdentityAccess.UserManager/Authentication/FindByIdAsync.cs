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
        /// Asynchronously finds a user by their unique identifier.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="userId">The unique identifier of the user. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The user entity if found; otherwise, <c>null</c>.
        /// Returns <c>null</c> if an exception occurs.
        /// </returns>
        public virtual async Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID must not be null or empty.", nameof(userId));

            try
            {
                return await _userManager.FindByIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to find user by ID {UserId}", userId);
                return null;
            }
        }
    }
}
