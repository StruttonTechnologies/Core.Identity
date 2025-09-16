using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously adds multiple claims to the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to add claims to. Must not be null.</param>
        /// <param name="claims">The claims to add. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            try
            {
                return await _userManager.AddClaimsAsync(user, claims);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add claims for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}