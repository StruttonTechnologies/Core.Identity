using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManagement
{
    public abstract partial class UserIdentityService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously gets a value indicating whether two-factor authentication is enabled for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user to check for two-factor authentication enablement. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if two-factor authentication is enabled; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                return await _userManager.GetTwoFactorEnabledAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get two-factor enabled status for user {UserId}", user?.Id);
                return false;
            }
        }
    }
}