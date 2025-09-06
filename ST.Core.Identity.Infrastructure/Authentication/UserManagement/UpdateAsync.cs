using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManagement
{
    public abstract partial class AuthenticationUserService<TUser>
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously updates the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to update. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>The updated user entity.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when user update fails due to validation or other errors.
        /// </exception>
        public virtual async Task<TUser> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to update user {UserId}: {Errors}", user.Id, errors);
                    throw new InvalidOperationException($"Failed to update user: {errors}");
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating user {UserId}", user?.Id);
                throw;
            }
        }
    }
}
