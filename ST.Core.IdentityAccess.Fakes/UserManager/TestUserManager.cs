using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ST.Core.Identity.Fakes.Models;
using ST.Core.IdentityAccess.Fakes.Stores;

namespace ST.Core.IdentityAccess.Fakes.UserManager
{
    /// <summary>
    /// Test implementation of <see cref="UserManager{TUser}"/> for <see cref="TestAppIdentityUser"/>.
    /// Uses in-memory store and dummy token provider for testing purposes.
    /// </summary>
    public class TestUserManager : UserManager<TestAppIdentityUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserManager"/> class.
        /// </summary>
        public TestUserManager(IUserStore<TestAppIdentityUser> store)
            : base(
                store,
                new FakeOptions(),
                new PasswordHasher<TestAppIdentityUser>(),
                new List<IUserValidator<TestAppIdentityUser>>(),
                new List<IPasswordValidator<TestAppIdentityUser>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new DummyServiceProvider(), 
                new LoggerFactory().CreateLogger<UserManager<TestAppIdentityUser>>())
        {
            RegisterTokenProvider(TokenOptions.DefaultProvider, new DummyTokenProvider());
        }

        /// <summary>
        /// Provides fake <see cref="IdentityOptions"/> for testing.
        /// </summary>
        private class FakeOptions : IOptions<IdentityOptions>
        {
            /// <inheritdoc/>
            public IdentityOptions Value => new IdentityOptions();
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
                if (serviceType == typeof(IUserTwoFactorTokenProvider<TestAppIdentityUser>))
                    return new DummyTokenProvider();

                return null;
            }
        }

        /// <summary>
        /// Dummy token provider for two-factor authentication.
        /// Always returns a valid token for testing.
        /// </summary>
        private class DummyTokenProvider : IUserTwoFactorTokenProvider<TestAppIdentityUser>
        {
            /// <inheritdoc/>
            public Task<string> GenerateAsync(string purpose, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
                => Task.FromResult("valid-token");

            /// <inheritdoc/>
            public Task<bool> ValidateAsync(string purpose, string token, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
                => Task.FromResult(token == "valid-token");

            /// <inheritdoc/>
            public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
                => Task.FromResult(true);
        }
    }
}