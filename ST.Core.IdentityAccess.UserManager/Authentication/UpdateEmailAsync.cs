using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Validators;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    public abstract partial class AuthenticationUserService<TUser, TKey>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        
        /// <summary>
        /// Asynchronously updates the email address for the specified user.
        /// Generates a change email token, applies the change, and logs any errors.
        /// </summary>
        /// <param name="user">The user entity whose email will be updated. Must not be null.</param>
        /// <param name="newEmail">The new email address to set. Must not be null or whitespace.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// An <see cref="IdentityResult"/> indicating the outcome of the operation.
        /// If the update fails, the result will contain error descriptions.
        /// </returns>
        public virtual async Task<IdentityResult> UpdateEmailAsync(TUser user, string newEmail, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(newEmail);

            try
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
                var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Email update failed for user {UserId}: {Errors}", user.Id, errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during email update for user {UserId}", user.Id);
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Exception occurred: {ex.Message}"
                });
            }
        }
    }
}