using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Authentication.Interfaces.User;
using ST.Core.Identity.Domain.Authorization.Interfaces;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManagement
{
        /// <summary>
        /// Provides base functionality for user services, including registration, lookup, and update operations.
        /// </summary>
        /// <typeparam name="TUser">The user entity type, derived from IdentityUser.</typeparam>
        public abstract partial class UserIdentityService<TUser> :
            IUserLookupService<TUser>,
            //IUserRegistration<TUser>,
            IUserUpdater<TUser>,
            IUserAuthenticationService<TUser>,
            IUserLockoutService<TUser>,
            IUserTwoFactorService<TUser>,
            IUserMaintenanceService<TUser>
            where TUser : IdentityUser, new()
        {
            /// <summary>
            /// The user manager instance used for user operations.
            /// </summary>
            protected readonly UserManager<TUser> _userManager;
            protected readonly ILogger<UserIdentityService<TUser>> _logger;


            /// <summary>
            /// Initializes a new instance of the <see cref="UserIdentityService{TUser}"/> class.
            /// </summary>
            /// <param name="userManager">The user manager to use for user operations.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="userManager"/> is null.</exception>
            protected UserIdentityService(UserManager<TUser> userManager, ILogger<UserIdentityService<TUser>> logger)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
        }
    }