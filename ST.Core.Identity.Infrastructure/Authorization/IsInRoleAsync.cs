using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser> where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously determines whether the specified user is in the given role.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to check. Must not be null.</param>
        /// <param name="role">The role to check for. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if the user is in the role; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> IsInRoleAsync(TUser user, string role, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(role))
                throw new ArgumentException("Role must not be null or empty.", nameof(role));

            try
            {
                return await _userManager.IsInRoleAsync(user, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check if user {UserId} is in role {Role}", user?.Id, role);
                return false;
            }
        }
    }
}