using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Application.Authentication.Services.RegistrerUser;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Testable
{
    public class TestableRegisterUserService : RegisterUserService<TestAppIdentityUser, Guid>
    {
        public TestableRegisterUserService(UserManager<TestAppIdentityUser> userManager, ILogger<RegisterUserService<TestAppIdentityUser, Guid>> logger)
            : base(userManager, logger) { }

        public Task<TestAppIdentityUser> ExposeCreateUserInstanceAsync(RegistrationRequestDto request) =>
            CreateUserInstanceAsync(request);

        public Task ExposeAssignRolesAsync(TestAppIdentityUser user, IList<string>? roles) =>
            AssignRolesAsync(user, roles);

        // etc...
    }


}
