using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Fakes.Factories;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Fakes.Tests.Factories
{
    [ExcludeFromCodeCoverage]
    public class TestIdentityUserFactoryTests
    {
        [Fact]
        public void CreateDefault_ReturnsUserWithExpectedDefaults()
        {
            StubUser user = TestAppUserIdentityFactory.CreateDefault();

            Assert.NotNull(user);
            Assert.Equal("test.user", user.UserName);
            Assert.True(user.EmailConfirmed);
            Assert.NotNull(user.SecurityStamp);
            Assert.NotNull(user.ConcurrencyStamp);
            Assert.False(user.LockoutEnabled);
            Assert.Equal(0, user.AccessFailedCount);
        }

        [Theory]
        [InlineData("alice")]
        [InlineData("bob")]
        public void Create_SetsUserNameAndEmail(string userName)
        {
            StubUser user = TestAppUserIdentityFactory.Create(userName);

            Assert.Equal(userName, user.UserName);
            Assert.Equal($"{userName}@example.com", user.Email);
        }

        [Fact]
        public void LockedOut_ReturnsUserWithLockoutProperties()
        {
            StubUser user = TestAppUserIdentityFactory.LockedOut();

            Assert.True(user.LockoutEnabled);
            Assert.True(user.LockoutEnd > DateTimeOffset.UtcNow.AddYears(99));
            Assert.Equal(5, user.AccessFailedCount);
        }

        [Theory]
        [InlineData("user1@example.com")]
        [InlineData("user2@example.com")]
        public void CreateWithEmail_SetsEmailAndConfirmed(string email)
        {
            StubUser user = TestAppUserIdentityFactory.CreateWithEmail(email);

            Assert.Equal(email, user.Email);
            Assert.True(user.EmailConfirmed);
        }

        [Theory]
        [InlineData("userA")]
        [InlineData("userB")]
        public void CreateWithUserName_SetsUserName(string userName)
        {
            StubUser user = TestAppUserIdentityFactory.CreateWithUserName(userName);

            Assert.Equal(userName, user.UserName);
        }
    }
}
