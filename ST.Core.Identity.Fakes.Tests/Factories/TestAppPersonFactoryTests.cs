using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Models;
using System;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Factories
{
    public class TestAppPersonFactoryTests
    {
        [Fact]
        public void CreateDefault_ReturnsPersonWithExpectedDefaults()
        {
            var person = TestAppPersonFactory.CreateDefault();

            Assert.NotNull(person);
            Assert.NotEqual(Guid.Empty, person.Id);
            Assert.Equal("Test", person.FirstName);
            Assert.Equal("Person", person.LastName);
            Assert.Equal("test.person@example.com", person.ContactEmail);
            Assert.Equal(1, person.RowVersion);
            Assert.True((DateTime.UtcNow - person.CreatedDate).TotalSeconds < 5);
            Assert.Null(person.ModifiedDate);
            Assert.Null(person.CreatedById);
            Assert.Null(person.ModifiedById);
            Assert.Null(person.CreatedBy);
            Assert.Null(person.ModifiedBy);
        }

        [Theory]
        [InlineData("e1e1e1e1-e1e1-4e1e-e1e1-e1e1e1e1e1e1")]
        [InlineData("f2f2f2f2-f2f2-4f2f-f2f2-f2f2f2f2f2f2")]
        public void CreateWithAudit_SetsCreatedByProperties(string guidString)
        {
            var createdById = Guid.Parse(guidString);
            var person = TestAppPersonFactory.CreateWithAudit(createdById);

            Assert.Equal(createdById, person.CreatedById);
            Assert.NotNull(person.CreatedBy);
            Assert.IsType<TestAppPerson>(person.CreatedBy);
        }

        [Theory]
        [InlineData("a3a3a3a3-a3a3-4a3a-a3a3-a3a3a3a3a3a3")]
        [InlineData("b4b4b4b4-b4b4-4b4b-b4b4-b4b4b4b4b4b4")]
        public void CreateModified_SetsModifiedProperties(string guidString)
        {
            var modifiedById = Guid.Parse(guidString);
            var person = TestAppPersonFactory.CreateModified(modifiedById);

            Assert.Equal(modifiedById, person.ModifiedById);
            Assert.NotNull(person.ModifiedDate);
            Assert.True((DateTime.UtcNow - person.ModifiedDate!.Value).TotalSeconds < 5);
            Assert.NotNull(person.ModifiedBy);
            Assert.IsType<TestAppPerson>(person.ModifiedBy);
        }

        [Fact]
        public void CreateFullAudit_SetsAllAuditProperties()
        {
            var createdById = Guid.NewGuid();
            var modifiedById = Guid.NewGuid();
            var person = TestAppPersonFactory.CreateFullAudit(createdById, modifiedById);

            Assert.Equal(createdById, person.CreatedById);
            Assert.NotNull(person.CreatedBy);
            Assert.Equal(modifiedById, person.ModifiedById);
            Assert.NotNull(person.ModifiedDate);
            Assert.True((DateTime.UtcNow - person.ModifiedDate!.Value).TotalSeconds < 5);
            Assert.NotNull(person.ModifiedBy);
            Assert.IsType<TestAppPerson>(person.CreatedBy);
            Assert.IsType<TestAppPerson>(person.ModifiedBy);
        }
    }
}