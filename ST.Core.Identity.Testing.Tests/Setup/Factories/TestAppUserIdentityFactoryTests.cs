using ST.Core.Identity.Testing.Setup.Factories;
using ST.Core.Identity.Testing.Setup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Testing.Tests.Setup.Factories
{
    public class TestAppUserIdentityFactoryTests
    {
        [Fact]
        public void CreateDefault_SetsExpectedDefaults()
        {
            var user = new ST.Core.Identity.Testing.Setup.Models.TestAppIdentityUser();
            var id = user.Id; // Should be Guid


            //var user = TestAppUserIdentityFactory.CreateDefault();

            Assert.NotEqual(string.Empty, user.Id);
            Assert.Equal("test.user", user.UserName);
            Assert.True(user.EmailConfirmed);
            Assert.Equal("Local", user.ProviderName);
            Assert.True(user.IsActive);
        }

        [Fact]
        public void CreateWithPerson_SetsPersonAndPersonId()
        {
            var person = new TestAppPerson { Id = Guid.NewGuid(), FirstName = "Shawn" };
            var user = TestAppUserIdentityFactory.CreateWithPerson(person);

            Assert.Equal(person.Id, user.PersonId);
            Assert.Equal(person, user.Person);
        }

        [Fact]
        public void LockedOut_SetsLockoutProperties()
        {
            var user = TestAppUserIdentityFactory.LockedOut();

            Assert.True(user.LockoutEnabled);
            Assert.True(user.LockoutEnd > DateTimeOffset.UtcNow);
            Assert.Equal(5, user.AccessFailedCount);
        }
    }


}
