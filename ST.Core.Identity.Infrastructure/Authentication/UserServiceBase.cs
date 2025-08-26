using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Domain.Authentication.Interfaces.User;

namespace ST.Core.Identity.Infrastructure.Authentication
{
    public abstract partial class UserServiceBase<TUser> :
        IUserRegistration<TUser>,
        IUserLookup<TUser>,
        IUserUpdater<TUser>
        where TUser : IdentityUser, new()
    {
        protected readonly UserManager<TUser> _userManager;

        protected UserServiceBase(UserManager<TUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
    }
}