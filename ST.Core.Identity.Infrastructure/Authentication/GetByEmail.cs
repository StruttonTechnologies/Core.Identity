using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Infrastructure.Authentication
{
    public abstract partial class UserServiceBase<TUser>
        where TUser : IdentityUser, new()
    {
        public virtual async Task<TUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            // Assumes _userManager.FindByEmailAsync exists and returns TUser
            return await _userManager.FindByEmailAsync(email);
        }
    }
}