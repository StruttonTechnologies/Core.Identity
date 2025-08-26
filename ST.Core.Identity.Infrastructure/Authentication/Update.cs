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
        /// Updates the specified user asynchronously.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The updated user entity.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when user update fails due to validation or other errors.
        /// </exception>
        public virtual async Task<TUser> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update user: {errors}");
            }
            return user;
        }
    }
}
