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
        /// Asynchronously confirms the email address for the specified user using the provided token.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity whose email will be confirmed. Must not be null.</param>
        /// <param name="token">The confirmation token. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> ConfirmEmailAsync(TUser user, string token, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token must not be null or empty.", nameof(token));

            try
            {
                return await _userManager.ConfirmEmailAsync(user, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to confirm email for user {UserId}", user.Id);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}