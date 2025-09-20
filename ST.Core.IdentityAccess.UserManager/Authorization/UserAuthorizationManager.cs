using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.IdentityAccess.Contracts.UserManager;
namespace ST.Core.IdentityAccess.UserManager.Authorization
{
    public abstract partial class UserAuthorizationManager<TUser> :
        IUserAuthorizationManager<TUser>
        where TUser : IdentityUser<Guid>, new()
    {
        /// <summary>
        /// The user manager instance used for user operations.
        /// </summary>
        protected readonly ILogger<UserAuthorizationManager<TUser>> _logger;
        protected readonly UserManager<TUser> _userManager;
        public UserAuthorizationManager(UserManager<TUser> userManager, ILogger<UserAuthorizationManager<TUser>> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
