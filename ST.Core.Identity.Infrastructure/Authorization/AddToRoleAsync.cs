using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser>
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously adds the specified user to a role.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to add to the role. Must not be null.</param>
        /// <param name="role">The role to add the user to. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> AddToRoleAsync(TUser user, string role, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(role))
                throw new ArgumentException("Role must not be null or empty.", nameof(role));

            try
            {
                return await _userManager.AddToRoleAsync(user, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add user {UserId} to role {Role}", user?.Id, role);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}