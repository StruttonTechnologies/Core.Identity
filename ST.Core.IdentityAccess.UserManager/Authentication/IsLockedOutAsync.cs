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
        /// Asynchronously determines whether the specified user is currently locked out.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user to check for lockout status. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if the user is locked out; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> IsLockedOutAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                return await _userManager.IsLockedOutAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check lockout status for user {UserId}", user?.Id);
                return false;
            }
        }
    }
}