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
        /// Asynchronously sets the lockout end date and time for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user to set the lockout end date for. Must not be null.</param>
        /// <param name="lockoutEnd">The lockout end date and time, or null to clear lockout.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                return await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set lockout end date for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}