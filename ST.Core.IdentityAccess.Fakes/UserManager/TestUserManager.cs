using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ST.Core.Identity.Stub.Entities;
using ST.Core.IdentityAccess.Fakes.Stores;

namespace ST.Core.IdentityAccess.Fakes.UserManager
{
    /// <summary>
    /// Test implementation of <see cref="UserManager{TUser}"/> for <see cref="StubUser"/>.
    /// Uses in-memory store and dummy token provider for testing purposes.
    /// </summary>
    public class TestUserManager : UserManager<StubUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserManager"/> class.
        /// </summary>
        public TestUserManager(IUserStore<StubUser> store)
            : base(
                store,
                new FakeOptions(),
                new PasswordHasher<StubUser>(),
                new List<IUserValidator<StubUser>>(),
                new List<IPasswordValidator<StubUser>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new DummyServiceProvider(), 
                new LoggerFactory().CreateLogger<UserManager<StubUser>>())
        {
            RegisterTokenProvider(TokenOptions.DefaultProvider, new DummyTokenProvider());
        }

        /// <summary>
        /// Provides fake <see cref="IdentityOptions"/> for testing.
        /// </summary>
        private class FakeOptions : IOptions<IdentityOptions>
        {
            /// <inheritdoc/>
            public IdentityOptions Value => new();
        }

        /// <summary>
        /// Dummy service provider for resolving token providers.
        /// </summary>
        private class DummyServiceProvider : IServiceProvider
        {
            /// <summary>
            /// Gets a service object of the specified type.
            /// </summary>
            /// <param name="serviceType">The type of service object to get.</param>
            /// <returns>The service object, or null if not found.</returns>
            public object? GetService(Type serviceType)
            {
                if (serviceType == typeof(IUserTwoFactorTokenProvider<StubUser>))
                    return new DummyTokenProvider();

                return null;
            }
        }

        /// <summary>
        /// Dummy token provider for two-factor authentication.
        /// Always returns a valid token for testing.
        /// </summary>
        private class DummyTokenProvider : IUserTwoFactorTokenProvider<StubUser>
        {
            /// <inheritdoc/>
            public Task<string> GenerateAsync(string purpose, UserManager<StubUser> manager, StubUser user)
                => Task.FromResult("valid-token");

            /// <inheritdoc/>
            public Task<bool> ValidateAsync(string purpose, string token, UserManager<StubUser> manager, StubUser user)
                => Task.FromResult(token == "valid-token");

            /// <inheritdoc/>
            public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<StubUser> manager, StubUser user)
                => Task.FromResult(true);
        }
    }
}