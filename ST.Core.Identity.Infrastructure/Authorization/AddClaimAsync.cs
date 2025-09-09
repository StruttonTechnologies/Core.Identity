using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously adds a claim to the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to add the claim to. Must not be null.</param>
        /// <param name="claim">The claim to add. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> AddClaimAsync(TUser user, Claim claim, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            try
            {
                return await _userManager.AddClaimAsync(user, claim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add claim for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}