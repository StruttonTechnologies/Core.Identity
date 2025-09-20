using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser> where TUser : IdentityUser<Guid>, new()
    {
        /// <summary>
        /// Asynchronously retrieves the users in the specified role.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="roleName">The name of the role to search for. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// A list of users in the specified role.
        /// Returns an empty list if an exception occurs.
        /// </returns>
        public virtual async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentException("Role name must not be null or empty.", nameof(roleName));

            try
            {
                return await _userManager.GetUsersInRoleAsync(roleName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get users in role {RoleName}", roleName);
                return new List<TUser>();
            }
        }
    }
}