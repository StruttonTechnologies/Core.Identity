using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Infrastructure.Authentication
{
    public abstract partial class UserServiceBase<TUser>
        where TUser : IdentityUser, new()
    {
        public virtual async Task<TUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            // Assumes _userManager.FindByNameAsync exists and returns TUser
            return await _userManager.FindByNameAsync(username);
        }
    }
}