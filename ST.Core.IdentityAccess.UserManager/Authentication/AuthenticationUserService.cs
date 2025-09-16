using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.IdentityAccess.Contracts.UserManager;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
        /// <summary>
        /// Provides base functionality for user services, including registration, lookup, and update operations.
        /// </summary>
        /// <typeparam name="TUser">The user entity type, derived from IdentityUser.</typeparam>
        public abstract partial class AuthenticationUserService<TUser> :
            IUserLockoutManager<TUser>,
            IUserLookupManager<TUser>,
            IUserManager<TUser>,
            IUserPasswordManager<TUser>,
            IUserTokenManager<TUser>,
            IUserTwoFactorManager<TUser>
            where TUser : IdentityUser, new()
        {
            /// <summary>
            /// The user manager instance used for user operations.
            /// </summary>
            protected readonly UserManager<TUser> _userManager;
            protected readonly ILogger<AuthenticationUserService<TUser>> _logger;


            /// <summary>
            /// Initializes a new instance of the <see cref="AuthenticationUserService{TUser}"/> class.
            /// </summary>
            /// <param name="userManager">The user manager to use for user operations.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="userManager"/> is null.</exception>
            protected AuthenticationUserService(UserManager<TUser> userManager, ILogger<AuthenticationUserService<TUser>> logger)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
        }
    }