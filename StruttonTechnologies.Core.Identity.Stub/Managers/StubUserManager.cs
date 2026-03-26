using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using StruttonTechnologies.Core.Identity.Stub.Entities;
using StruttonTechnologies.Core.Identity.Stub.Stores;

namespace StruttonTechnologies.Core.Identity.Stub.Managers
{
    /// <summary>
    /// Stubbed UserManager for testing Identity flows without EF or external dependencies.
    /// </summary>
    public sealed class StubUserManager : UserManager<StubUser>
    {
        private readonly StubUserStore _store;
        private readonly LoggerFactory _loggerFactory;

        private StubUserManager(
    StubUserStore store,
    LoggerFactory loggerFactory,
    ILogger<UserManager<StubUser>> logger)
    : base(
        store: store,
        optionsAccessor: new OptionsWrapper<IdentityOptions>(new IdentityOptions()),
        passwordHasher: new PasswordHasher<StubUser>(),
        userValidators: [new UserValidator<StubUser>()],
        passwordValidators: [new PasswordValidator<StubUser>()],
        keyNormalizer: new UpperInvariantLookupNormalizer(),
        errors: new IdentityErrorDescriber(),
        services: null!,
        logger: logger)
        {
            _store = store;
            _loggerFactory = loggerFactory;
        }

#pragma warning disable CA2000 // Ownership transferred to StubUserManager and disposed in Dispose(bool)
        public static StubUserManager Create()
        {
            StubUserStore? store = null;
            LoggerFactory? loggerFactory = null;

            try
            {
                store = new StubUserStore();
                loggerFactory = new LoggerFactory();

                ILogger<UserManager<StubUser>> logger =
                    loggerFactory.CreateLogger<UserManager<StubUser>>();

                return new StubUserManager(store, loggerFactory, logger);
            }
            catch
            {
                store?.Dispose();
                loggerFactory?.Dispose();
                throw;
            }
        }
#pragma warning restore CA2000

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _store?.Dispose();
                _loggerFactory?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
