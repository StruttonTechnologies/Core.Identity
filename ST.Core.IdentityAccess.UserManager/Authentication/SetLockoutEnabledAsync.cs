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
        /// Asynchronously sets a value indicating whether lockout is enabled for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user to set lockout enablement for. Must not be null.</param>
        /// <param name="enabled">True to enable lockout; false to disable.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                return await _userManager.SetLockoutEnabledAsync(user, enabled);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set lockout enabled for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}