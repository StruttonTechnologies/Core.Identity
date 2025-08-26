using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ST.Core.Identity.Infrastructure.Authentication
{
    public abstract partial class UserServiceBase<TUser>
        where TUser : IdentityUser, new()
    {
        public virtual async Task<IEnumerable<TUser>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Assumes _userManager.Users is IQueryable<TUser>
            return await Task.FromResult(_userManager.Users.ToList());
        }
    }
}
