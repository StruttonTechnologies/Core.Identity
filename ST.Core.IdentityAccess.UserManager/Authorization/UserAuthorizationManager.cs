using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Interfaces.UserManager;
using ST.Core.IdentityAccess.UserManager.Authentication;
using ST.Core.Registration.Attributes;
namespace ST.Core.IdentityAccess.UserManager.Authorization
{
    [AutoRegister(ServiceLifetime.Singleton)]
    public abstract partial class UserAuthorizationManager<TUser, TKey> :
        IUserAuthorizationManager<TUser, TKey>
         where TUser : IdentityUser<TKey>, new()
         where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The user manager instance used for user operations.
        /// </summary>
        protected readonly UserManager<TUser> _userManager;
        protected readonly ILogger<AuthenticationUserService<TUser, TKey>> _logger;


        protected UserAuthorizationManager(UserManager<TUser> userManager, ILogger<UserAuthorizationManager<TUser, TKey>> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
