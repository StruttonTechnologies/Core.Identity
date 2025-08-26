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
        /// Deletes the specified user asynchronously.
        /// </summary>
        /// <param name="user">The user entity to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>True if the user was deleted successfully; otherwise, false.</returns>
        public virtual async Task<bool> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}