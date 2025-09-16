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
        /// Asynchronously retrieves the access failed count for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity for which to retrieve the access failed count. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The number of failed access attempts for the user.
        /// Returns <c>0</c> if an exception occurs.
        /// </returns>
        public virtual async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                return await _userManager.GetAccessFailedCountAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get access failed count for user {UserId}", user?.Id);
                return 0;
            }
        }
    }
}