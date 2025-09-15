using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Infrastructure.Tests.SetUp.Models;
using ST.Core.Identity.Testing.Setup.Models;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.UserManagement.Setup
{
    public abstract class AuthenticationUserServiceTestBase
    {
        protected readonly TestUserManager UserManager;
        protected readonly ILogger<AuthenticationUserService<TestAppIdentityUser>> Logger;
        protected readonly AuthenticationUserService<TestAppIdentityUser> Service;

        protected AuthenticationUserServiceTestBase()
        {
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