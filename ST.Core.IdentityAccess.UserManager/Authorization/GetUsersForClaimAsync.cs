using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser> where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously retrieves the users associated with the specified claim.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="claim">The claim to search for. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// A list of users associated with the claim.
        /// Returns an empty list if an exception occurs.
        /// </returns>
        public virtual async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            try
            {
                return await _userManager.GetUsersForClaimAsync(claim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get users for claim {ClaimType}:{ClaimValue}", claim.Type, claim.Value);
                return new List<TUser>();
            }
        }
    }
}