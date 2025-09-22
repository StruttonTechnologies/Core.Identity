using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.IdentityAccess.Fakes.Stores;
using ST.Core.Identity.Fakes.Models;
using ST.Core.IdentityAccess.UserManager;
using ST.Core.IdentityAccess.UserManager.Authentication;

namespace ST.Core.IdentityAccess.Fakes.UserManager
{
    /// <summary>
    /// Base class for testing <see cref="AuthenticationUserService{TUser}"/>.
    /// Provides a test-safe <see cref="UserManager{TUser}"/> and logger.
    /// </summary>
    public abstract class AuthenticationUserServiceTestBase
    {
        protected readonly TestUserManager UserManager;
        protected readonly ILogger<AuthenticationUserService<TestAppIdentityUser, Guid>> Logger;
        protected readonly AuthenticationUserService<TestAppIdentityUser, Guid> Service;

        protected AuthenticationUserServiceTestBase()
        {
            var store = new InMemoryUserStore();
            UserManager = new TestUserManager(store);


            Logger = new LoggerFactory().CreateLogger<AuthenticationUserService<TestAppIdentityUser, Guid>>();
            Service = new TestableAuthenticationUserService(UserManager, Logger);
        }

        protected class TestableAuthenticationUserService : AuthenticationUserService<TestAppIdentityUser, Guid>
        {
            public TestableAuthenticationUserService(
                UserManager<TestAppIdentityUser> userManager,
                ILogger<AuthenticationUserService<TestAppIdentityUser, Guid>> logger)
                : base(userManager, logger) { }
        }
    }
}