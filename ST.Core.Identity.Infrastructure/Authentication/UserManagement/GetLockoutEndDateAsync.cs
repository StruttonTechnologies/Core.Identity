using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManagement
{
    public abstract partial class AuthenticationUserService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously gets the lockout end date and time for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user whose lockout end date to retrieve. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The lockout end date and time, or <c>null</c> if not locked out or if an exception occurs.
        /// </returns>
        public virtual async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                return await _userManager.GetLockoutEndDateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get lockout end date for user {UserId}", user?.Id);
                return null;
            }
        }
    }
}