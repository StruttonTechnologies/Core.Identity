using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ST.Core.Identity.Infrastructure.Authentication
{
    public abstract partial class UserServiceBase<TUser>
        where TUser : IdentityUser, new()
    {
        public virtual async Task<TUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Assumes _userManager.FindByIdAsync exists and returns TUser
            return await _userManager.FindByIdAsync(userId.ToString());
        }
    }
}
