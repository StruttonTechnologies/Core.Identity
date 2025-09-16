using ST.Core.Identity.Testing.Setup.Factories;
using ST.Core.Identity.Testing.Setup.Models;
using System;
using Xunit;

namespace ST.Core.Identity.Testing.Tests.Setup.Factories
{
    /// <summary>
    /// Unit tests for <see cref="TestAppUserIdentityFactory"/>.
    /// Validates default user creation and specialized factory methods.
    /// </summary>
    public class TestAppUserIdentityFactoryTests
    {
        [Fact]
        public void CreateDefault_SetsExpectedDefaults()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();

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

        [Fact]
        public void CreateWithEmail_SetsEmail()
        {
            var user = TestAppUserIdentityFactory.CreateWithEmail("shawn@example.com");

            Assert.Equal("shawn@example.com", user.Email);
            Assert.True(user.EmailConfirmed);
        }

        [Fact]
        public void CreateWithUserName_SetsUserName()
        {
            var user = TestAppUserIdentityFactory.CreateWithUserName("shawn");

            Assert.Equal("shawn", user.UserName);
        }

        [Fact]
        public void CreateWithProvider_SetsProviderName()
        {
            var user = TestAppUserIdentityFactory.CreateWithProvider("GitHub");

            Assert.Equal("GitHub", user.ProviderName);
        }
    }
}