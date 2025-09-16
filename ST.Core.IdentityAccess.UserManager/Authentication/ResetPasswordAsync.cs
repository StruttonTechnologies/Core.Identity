using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    public abstract partial class AuthenticationUserService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously resets the password for the specified user using the provided token and new password.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity whose password will be reset. Must not be null.</param>
        /// <param name="token">The password reset token. Must not be null or empty.</param>
        /// <param name="newPassword">The new password to set. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token must not be null or empty.", nameof(token));
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentException("New password must not be null or empty.", nameof(newPassword));

            try
            {
                return await _userManager.ResetPasswordAsync(user, token, newPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reset password for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}