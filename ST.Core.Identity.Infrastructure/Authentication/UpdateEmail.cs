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
        /// Updates the email for the specified user asynchronously.
        /// </summary>
        /// <param name="user">The user entity whose email will be updated.</param>
        /// <param name="newEmail">The new email address.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>True if the email was updated successfully; otherwise, false.</returns>
        public virtual async Task<bool> UpdateEmailAsync(TUser user, string newEmail, CancellationToken cancellationToken = default)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            return result.Succeeded;
        }
    }
}