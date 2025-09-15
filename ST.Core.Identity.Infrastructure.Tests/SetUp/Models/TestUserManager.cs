using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ST.Core.Identity.Testing.Setup.Models;
using System.Collections.Generic;

namespace ST.Core.Identity.Infrastructure.Tests.SetUp.Models
{
    public class TestUserManager : UserManager<TestAppIdentityUser>
    {
        public TestUserManager()
            : base(
                new MockUserStore(),
                new FakeOptions(),
                new PasswordHasher<TestAppIdentityUser>(),
                new List<IUserValidator<TestAppIdentityUser>>(),
                new List<IPasswordValidator<TestAppIdentityUser>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null!, // IServiceProvider
                new LoggerFactory().CreateLogger<UserManager<TestAppIdentityUser>>())
        { }

        private class FakeOptions : IOptions<IdentityOptions>
        {
            public IdentityOptions Value => new IdentityOptions();
        }
    }
}