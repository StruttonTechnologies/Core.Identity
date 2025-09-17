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
        /// Asynchronously changes the password for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity whose password will be changed. Must not be null.</param>
        /// <param name="currentPassword">The user's current password. Must not be null or empty.</param>
        /// <param name="newPassword">The new password to set. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// Returns <see cref="IdentityResult.Failed"/> if an exception occurs.
        /// </returns>
        public virtual async Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(currentPassword, nameof(currentPassword));
            ArgumentException.ThrowIfNullOrEmpty(newPassword, nameof(newPassword));

            try
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (!result.Succeeded)
                {
                    var wrappedErrors = result.Errors.Select(e => new IdentityError
                    {
                        Description = $"Exception: {e.Description}"
                    });

                    return IdentityResult.Failed(wrappedErrors.ToArray());
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change password for user {UserId}", user.Id);
                return IdentityResult.Failed(new IdentityError { Description = $"Exception: {ex.Message}" });
            }
        }
    }
}