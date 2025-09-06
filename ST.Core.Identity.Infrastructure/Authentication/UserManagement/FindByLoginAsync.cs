using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManagement
{
    public abstract partial class AuthenticationUserService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously finds a user by their external login information.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="loginProvider">The login provider (e.g., Google, Facebook). Must not be null or empty.</param>
        /// <param name="providerKey">The unique provider key for the user. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The user entity if found; otherwise, <c>null</c>.
        /// Returns <c>null</c> if an exception occurs.
        /// </returns>
        public virtual async Task<TUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(loginProvider))
                throw new ArgumentException("Login provider must not be null or empty.", nameof(loginProvider));
            if (string.IsNullOrEmpty(providerKey))
                throw new ArgumentException("Provider key must not be null or empty.", nameof(providerKey));

            try
            {
                return await _userManager.FindByLoginAsync(loginProvider, providerKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to find user by login {LoginProvider}/{ProviderKey}", loginProvider, providerKey);
                return null;
            }
        }
    }
}