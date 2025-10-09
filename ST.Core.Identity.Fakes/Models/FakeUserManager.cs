using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ST.Core.Identity.Stub.Entities;
using ST.Core.Identity.Stub.Stores;

namespace ST.Core.Identity.Fakes.Models
{
    /// <summary>
    /// A fake <see cref="UserManager{TUser}"/> for <see cref="StubUser"/>.
    /// Uses an in-memory store and dummy token provider for realistic testing.
    /// </summary>
    public class FakeUserManager : UserManager<StubUser>
    {
        public FakeUserManager()
            : base(
                new InMemoryUserStore<StubUser>(),
                new FakeOptions(),
                new PasswordHasher<StubUser>(),
                new List<IUserValidator<StubUser>>(),
                new List<IPasswordValidator<StubUser>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new FakeServiceProvider(),
                new LoggerFactory().CreateLogger<UserManager<StubUser>>())
        {
            RegisterTokenProvider(TokenOptions.DefaultProvider, new FakeTokenProvider());
        }

        private class FakeOptions : IOptions<IdentityOptions>
        {
            public IdentityOptions Value => new IdentityOptions();
        }

        private class FakeServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType)
            {
                if (serviceType == typeof(IUserTwoFactorTokenProvider<StubUser>))
                    return new FakeTokenProvider();
                return null;
            }
        }

        private class FakeTokenProvider : IUserTwoFactorTokenProvider<StubUser>
        {
            public Task<string> GenerateAsync(string purpose, UserManager<StubUser> manager, StubUser user)
                => Task.FromResult(Guid.NewGuid().ToString("N")); // unique per call

            public Task<bool> ValidateAsync(string purpose, string token, UserManager<StubUser> manager, StubUser user)
                => Task.FromResult(!string.IsNullOrEmpty(token));

            public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<StubUser> manager, StubUser user)
                => Task.FromResult(true);
        }
    }
}
