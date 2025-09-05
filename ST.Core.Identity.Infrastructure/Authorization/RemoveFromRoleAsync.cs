using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authorization
{
    public abstract partial class UserAuthorizationService<TUser> where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously removes the specified user from a role.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to remove from the role. Must not be null.</param>
        /// <param name="role">The role to remove the user from. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(role))
                throw new ArgumentException("Role must not be null or empty.", nameof(role));

            try
            {
                return await _userManager.RemoveFromRoleAsync(user, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove user {UserId} from role {Role}", user?.Id, role);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}