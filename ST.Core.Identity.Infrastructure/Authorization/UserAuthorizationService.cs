using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Authorization.Interfaces;
using ST.Core.Identity.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authorization
{
    public abstract partial class UserAuthorizationService<TUser> :
        IUserAuthorizationService<TUser>
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// The user manager instance used for user operations.
        /// </summary>
        protected readonly ILogger<UserAuthorizationService<TUser>> _logger;
        protected readonly UserManager<TUser> _userManager;
        public UserAuthorizationService(UserManager<TUser> userManager, ILogger<UserAuthorizationService<TUser>> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
