using ST.Core.Identity.Fakes.Models;

namespace ST.Core.Identity.Fakes.Factories
{
    /// <summary>
    /// Factory for creating test-safe <see cref="TestAppIdentityUser"/> instances.
    /// Provides default configurations and targeted overrides for identity-related tests.
    /// </summary>
    public static class TestAppUserIdentityFactory
    {
        /// <summary>
        /// Creates a default user with safe, realistic values.
        /// </summary>
        public static TestAppIdentityUser CreateDefault()
        {
            return new TestAppIdentityUser
            {
                Id = Guid.NewGuid(),
                UserName = "test.user",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                
                ProviderName = "Local",
                IsActive = true,
                RowVersion = 1,
                CreateDate = DateTime.UtcNow,
                ModifiedDate = null,
                PersonId = Guid.NewGuid()
            };
        }

        /// <summary>
        /// Creates a user with a specified username and derived email.
        /// </summary>
        public static TestAppIdentityUser Create(string userName)
        {
            var user = CreateDefault();
            user.UserName = userName;
            user.Email = $"{userName}@example.com";
            return user;
        }

        /// <summary>
        /// Creates a user and attaches the specified <see cref="TestAppPerson"/>.
        /// </summary>
        public static TestAppIdentityUser CreateWithPerson(TestAppPerson person)
        {
            var user = CreateDefault();
            user.PersonId = person.Id;
            user.Person = person;
            return user;
        }



        /// <summary>
        /// Creates a user marked as locked out with elevated failure count.
        /// </summary>
        public static TestAppIdentityUser LockedOut()
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
        public static TestAppIdentityUser CreateWithEmail(string email)
        {
            var user = CreateDefault();
            user.Email = email;
            user.EmailConfirmed = true;
            return user;
        }

        /// <summary>
        /// Creates a user with a specified username.
        /// </summary>
        public static TestAppIdentityUser CreateWithUserName(string userName)
        {
            var user = CreateDefault();
            user.UserName = userName;
            return user;
        }

        /// <summary>
        /// Creates a user with a specified external provider name.
        /// </summary>
        public static TestAppIdentityUser CreateWithProvider(string providerName)
        {
            var user = CreateDefault();
            user.ProviderName = providerName;
            return user;
        }

        /// <summary>
        /// Creates a user marked as inactive.
        /// </summary>
        public static TestAppIdentityUser CreateInactive()
        {
            var user = CreateDefault();
            user.IsActive = false;
            return user;
        }
    }
}