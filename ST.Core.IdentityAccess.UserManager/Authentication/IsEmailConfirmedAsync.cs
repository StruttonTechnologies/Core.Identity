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
        /// Asynchronously checks if the specified user's email is confirmed.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to check. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if the user's email is confirmed; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> IsEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null)
                    throw new InvalidOperationException($"User {user.Id} not found in store.");

                return await _userManager.IsEmailConfirmedAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check email confirmation for user {UserId}", user?.Id);
                return false;
            }
        }
    }
}