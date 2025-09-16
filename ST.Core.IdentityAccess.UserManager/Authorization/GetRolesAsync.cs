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
        /// Asynchronously retrieves the roles for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to retrieve roles for. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// A list of roles for the user.
        /// Returns an empty list if an exception occurs.
        /// </returns>
        public virtual async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get roles for user {UserId}", user?.Id);
                return new List<string>();
            }
        }
    }
}