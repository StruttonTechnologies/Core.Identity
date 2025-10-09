using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Stub.Entities;

namespace ST.Core.Identity.Fakes.Factories
{
    /// <summary>
    /// Factory for creating test-safe <see cref="StubUser"/> instances.
    /// Provides default configurations and targeted overrides for identity-related tests.
    /// </summary>
    public static class TestAppUserIdentityFactory
    {
        /// <summary>
        /// Creates a default user with safe, realistic values.
        /// </summary>
        public static StubUser CreateDefault()
        {
            return new StubUser
            {
                Id = Guid.NewGuid(),
                UserName = "test.user",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                
            };
        }

        /// <summary>
        /// Creates a user with a specified username and derived email.
        /// </summary>
        public static StubUser Create(string userName)
        {
            var user = CreateDefault();
            user.UserName = userName;
            user.Email = $"{userName}@example.com";
            return user;
        }


        /// <summary>
        /// Creates a user marked as locked out with elevated failure count.
        /// </summary>
        public static StubUser LockedOut()
        {
            var user = CreateDefault();
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            user.AccessFailedCount = 5;
            return user;
        }

        /// <summary>
        /// Creates a user with a specified email address.
        /// </summary>
        public static StubUser CreateWithEmail(string email)
        {
            var user = CreateDefault();
            user.Email = email;
            user.EmailConfirmed = true;
            return user;
        }

        /// <summary>
        /// Creates a user with a specified username.
        /// </summary>
        public static StubUser CreateWithUserName(string userName)
        {
            var user = CreateDefault();
            user.UserName = userName;
            return user;
        }
    }
}