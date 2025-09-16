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
        /// Asynchronously updates the password for the specified user.
        /// Generates a password reset token, applies the change, and logs any errors.
        /// </summary>
        /// <param name="user">The user entity whose password will be updated. Must not be null.</param>
        /// <param name="newPassword">The new password to set. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// An <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// If the update fails, the result will contain error descriptions.
        /// </returns>
        public virtual async Task<IdentityResult> UpdatePasswordAsync(TUser user, string newPassword, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(newPassword);

            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Password update failed for user {UserId}: {Errors}", user.Id, errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating password for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Exception occurred: {ex.Message}"
                });
            }
        }
    }
}