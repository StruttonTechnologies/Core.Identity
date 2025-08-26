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
        /// Updates the username for the specified user asynchronously.
        /// </summary>
        /// <param name="user">The user entity whose username will be updated.</param>
        /// <param name="newUsername">The new username.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>True if the username was updated successfully; otherwise, false.</returns>
        public virtual async Task<bool> UpdateUsernameAsync(TUser user, string newUsername, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.SetUserNameAsync(user, newUsername);
            return result.Succeeded;
        }
    }
}