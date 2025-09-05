using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Infrastructure.Authentication;
using ST.Core.Identity.Infrastructure.Authentication.UserManagement;
using ST.Core.Identity.Testing.Toolkit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Tests.Authentication.Users
{
    /// <summary>
    /// Concrete subclass of UserIdentityService for testing.
    /// Enables instantiation with mock dependencies.
    /// </summary>
    public class TestUserIdentityService : UserIdentityService<TestUser>
    {
        public TestUserIdentityService(
            UserManager<TestUser> userManager,
            ILogger<UserIdentityService<TestUser>> logger)
            : base(userManager, logger)
        {
        }

        // No overrides—this class simply enables testing the real logic.
    }
}
