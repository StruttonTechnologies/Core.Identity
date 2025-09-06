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
        /// Asynchronously deletes the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to delete. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if the user was deleted successfully; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to delete user {UserId}: {Errors}", user.Id, errors);
                }
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting user {UserId}", user?.Id);
                return false;
            }
        }
    }
}