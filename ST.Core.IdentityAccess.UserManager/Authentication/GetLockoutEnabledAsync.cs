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
        /// Asynchronously gets a value indicating whether lockout is enabled for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user to check for lockout enablement. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if lockout is enabled; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                return await _userManager.GetLockoutEnabledAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get lockout enabled status for user {UserId}", user.Id);
                return false;
            }
        }
    }
}