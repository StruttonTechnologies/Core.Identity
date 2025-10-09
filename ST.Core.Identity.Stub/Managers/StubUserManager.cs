using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ST.Core.Identity.Stub.Entities;
using ST.Core.Identity.Stub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Stub.Managers
{
    /// <summary>
    /// Stubbed UserManager for testing Identity flows without EF or external dependencies.
    /// </summary>
    public sealed class StubUserManager : UserManager<StubUser>
    {
        public StubUserManager()
            : base(
                store: new StubUserStore(),
                optionsAccessor: new OptionsWrapper<IdentityOptions>(new IdentityOptions()),
                passwordHasher: new PasswordHasher<StubUser>(),
                userValidators: new List<IUserValidator<StubUser>> { new UserValidator<StubUser>() },
                passwordValidators: new List<IPasswordValidator<StubUser>> { new PasswordValidator<StubUser>() },
                keyNormalizer: new UpperInvariantLookupNormalizer(),
                errors: new IdentityErrorDescriber(),
                services: null!,
                logger: new LoggerFactory().CreateLogger<UserManager<StubUser>>()
            )
        {
        }
    }
}
