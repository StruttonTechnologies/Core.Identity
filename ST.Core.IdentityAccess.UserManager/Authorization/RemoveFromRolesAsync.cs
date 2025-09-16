using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser> where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously removes the specified user from multiple roles.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to remove from the roles. Must not be null.</param>
        /// <param name="roles">The roles to remove the user from. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> RemoveFromRolesAsync(TUser user, IEnumerable<string> roles, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (roles == null)
                throw new ArgumentNullException(nameof(roles));

            try
            {
                return await _userManager.RemoveFromRolesAsync(user, roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove user {UserId} from roles {Roles}", user?.Id, roles);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}