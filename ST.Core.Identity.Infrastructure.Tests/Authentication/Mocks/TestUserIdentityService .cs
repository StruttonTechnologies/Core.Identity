using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Infrastructure.Tests.SetUp.Models;
using ST.Core.Identity.Testing.Setup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Mocks
{
    /// <summary>
    /// Concrete subclass of UserIdentityService for testing purposes.
    /// Enables instantiation of the abstract base with mockable dependencies.
    /// </summary>
    public class TestUserIdentityService : AuthenticationUserService<TestAppIdentityUser>
    {
        public TestUserIdentityService(
            UserManager<TestAppIdentityUser> userManager,
            ILogger<AuthenticationUserService<TestAppIdentityUser>> logger)
            : base(userManager, logger)
        {
        }

        // No overrides—this class simply enables testing the real logic.
    }
}
