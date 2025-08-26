using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication
{
    public abstract partial class UserServiceBase<TUser>
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Updates the password for the specified user asynchronously.
        /// </summary>
        /// <param name="user">The user entity whose password will be updated.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>True if the password was updated successfully; otherwise, false.</returns>
        public virtual async Task<bool> UpdatePasswordAsync(TUser user, string newPassword, CancellationToken cancellationToken = default)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
    }
}