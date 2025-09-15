using ST.Core.Identity.Testing.Setup.Models;

namespace ST.Core.Identity.Testing.Setup.Factories
{
    public static class TestAppUserIdentityFactory
    {
        public static TestAppIdentityUser CreateDefault()
        {
            return new TestAppIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "test.user",
                EmailConfirmed = true,
                PasswordHash = "hashed-password",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                AccessFailedCount = 0,

                ProviderName = "Local",
                PersonId = Guid.NewGuid(),
                IsActive = true,
                RowVersion = 1,
                CreateDate = DateTime.UtcNow,
                ModifiedDate = null
            };
        }

        public static TestAppIdentityUser Create(string userName)
        {
            var user = CreateDefault();
            user.UserName = userName;
            user.Email = $"{userName}@example.com";
            return user;
        }



        public static TestAppIdentityUser CreateWithPerson(TestAppPerson person)
        {
            var user = CreateDefault();
            user.PersonId = person.Id;
            user.Person = person;
            return user;
        }

        public static TestAppIdentityUser LockedOut()
        {
            var user = CreateDefault();
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            user.AccessFailedCount = 5;
            return user;
        }
    }
}