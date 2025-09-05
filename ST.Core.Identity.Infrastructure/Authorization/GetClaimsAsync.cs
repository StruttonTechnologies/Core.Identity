using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authorization
{
    public abstract partial class UserAuthorizationService<TUser> where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously retrieves the claims for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to retrieve claims for. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// A list of claims for the user.
        /// Returns an empty list if an exception occurs.
        /// </returns>
        public virtual async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                return await _userManager.GetClaimsAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get claims for user {UserId}", user?.Id);
                return new List<Claim>();
            }
        }
    }
}