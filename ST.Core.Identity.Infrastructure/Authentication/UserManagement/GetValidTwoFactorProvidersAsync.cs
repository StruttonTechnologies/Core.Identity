using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManagement
{
    public abstract partial class UserIdentityService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously retrieves a list of valid two-factor authentication providers for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user to retrieve valid two-factor providers for. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// A list of valid two-factor provider names.
        /// Returns an empty list if an exception occurs.
        /// </returns>
        public virtual async Task<IList<string>> GetValidTwoFactorProvidersAsync(TUser user, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                return await _userManager.GetValidTwoFactorProvidersAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get valid two-factor providers for user {UserId}", user?.Id);
                return new List<string>();
            }
        }
    }
}