using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Interfaces.UserManager;
using ST.Core.Registration.Attributes;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    [AutoRegister(ServiceLifetime.Singleton)]
    /// <summary>
    /// Provides base functionality for user services, including registration, lookup, and update operations.
    /// </summary>
    /// <typeparam name="TUser">The user entity type, derived from IdentityUser.</typeparam>
    public abstract partial class AuthenticationUserService<TUser, TKey> :
        IUserLockoutManager<TUser, TKey>,
        IUserLookupManager<TUser, TKey>,
        IUserManager<TUser, TKey>,
        IUserPasswordManager<TUser, TKey>,
        IUserTokenManager<TUser, TKey>,
        IUserTwoFactorManager<TUser, TKey>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The user manager instance used for user operations.
        /// </summary>
        protected readonly UserManager<TUser> _userManager;
        protected readonly ILogger<AuthenticationUserService<TUser, TKey>> _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationUserService{TUser, TKey}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager to use for user operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="userManager"/> is null.</exception>
        protected AuthenticationUserService(UserManager<TUser> userManager, ILogger<AuthenticationUserService<TUser, TKey>> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}