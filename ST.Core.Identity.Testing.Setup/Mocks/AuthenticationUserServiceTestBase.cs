using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Infrastructure.Tests.SetUp.Models;
using ST.Core.Identity.Testing.Setup.Models;
using ST.Core.Identity.UserManager.Testing.Setup.Stores;
using ST.Core.IdentityAccess.UserManager;

namespace ST.Core.Identity.UserManager.Testing.SetUp.Mocks
{
    /// <summary>
    /// Base class for testing <see cref="AuthenticationUserService{TUser}"/>.
    /// Provides a test-safe <see cref="UserManager{TUser}"/> and logger.
    /// </summary>
    public abstract class AuthenticationUserServiceTestBase
    {
        protected readonly TestUserManager UserManager;
        protected readonly ILogger<AuthenticationUserService<TestAppIdentityUser>> Logger;
        protected readonly AuthenticationUserService<TestAppIdentityUser> Service;

        protected AuthenticationUserServiceTestBase()
        {
            var store = new InMemoryUserStore();
            UserManager = new TestUserManager();
            Logger = new LoggerFactory().CreateLogger<AuthenticationUserService<TestAppIdentityUser>>();
            Service = new TestableAuthenticationUserService(UserManager, Logger);
        }

        protected class TestableAuthenticationUserService : AuthenticationUserService<TestAppIdentityUser>
        {
            public TestableAuthenticationUserService(
                UserManager<TestAppIdentityUser> userManager,
                ILogger<AuthenticationUserService<TestAppIdentityUser>> logger)
                : base(userManager, logger) { }
        }
    }
}