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
        /// Asynchronously updates the username for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity whose username will be updated. Must not be null.</param>
        /// <param name="newUsername">The new username to set. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// An <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// If the update fails, the result will contain error descriptions.
        /// </returns>
        public virtual async Task<IdentityResult> UpdateUsernameAsync(TUser user, string newUsername, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(newUsername);

            try
            {
                var result = await _userManager.SetUserNameAsync(user, newUsername);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Username update failed for user {UserId}: {Errors}", user.Id, errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating username for user {UserId}", user?.Id);
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Exception occurred: {ex.Message}"
                });
            }
        }
    }
}