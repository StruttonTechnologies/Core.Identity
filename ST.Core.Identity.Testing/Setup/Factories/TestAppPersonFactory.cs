using ST.Core.Identity.Testing.Setup.Models;

namespace ST.Core.Identity.Testing.Setup.Factories
{
    /// <summary>
    /// Factory for creating test-safe instances of TestAppPerson.
    /// </summary>
    public static class TestAppPersonFactory
    {
        public static TestAppPerson CreateDefault()
        {
            return new TestAppPerson
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "Person",
                ContactEmail = "test.person@example.com",
                RowVersion = 1,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static TestAppPerson CreateWithAudit(Guid createdById, TestAppPerson? createdBy = null)
        {
            var person = CreateDefault();
            person.CreatedById = createdById;
            person.CreatedBy = createdBy ?? CreateDefault();
            return person;
        }

        public static TestAppPerson CreateModified(Guid modifiedById, TestAppPerson? modifiedBy = null)
        {
            var person = CreateDefault();
            person.ModifiedDate = DateTime.UtcNow;
            person.ModifiedById = modifiedById;
            person.ModifiedBy = modifiedBy ?? CreateDefault();
            return person;
        }

        public static TestAppPerson CreateFullAudit(Guid createdById, Guid modifiedById)
        {
            var person = CreateDefault();
            person.CreatedById = createdById;
            person.CreatedBy = CreateDefault();
            person.ModifiedDate = DateTime.UtcNow;
            person.ModifiedById = modifiedById;
            person.ModifiedBy = CreateDefault();
            return person;
        }
    }
}
