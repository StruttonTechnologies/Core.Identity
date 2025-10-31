using ST.Core.Identity.Fakes.Factories;
using System;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Factories
{
    public class TestIdentityUserFactoryTests
    {
        [Fact]
        public void CreateDefault_ReturnsUserWithExpectedDefaults()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();

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
            var user = TestAppUserIdentityFactory.Create(userName);

            Assert.Equal(userName, user.UserName);
            Assert.Equal($"{userName}@example.com", user.Email);
        }

        

        [Fact]
        public void LockedOut_ReturnsUserWithLockoutProperties()
        {
            var user = TestAppUserIdentityFactory.LockedOut();

            Assert.True(user.LockoutEnabled);
            Assert.True(user.LockoutEnd > DateTimeOffset.UtcNow.AddYears(99));
            Assert.Equal(5, user.AccessFailedCount);
        }

        [Theory]
        [InlineData("user1@example.com")]
        [InlineData("user2@example.com")]
        public void CreateWithEmail_SetsEmailAndConfirmed(string email)
        {
            var user = TestAppUserIdentityFactory.CreateWithEmail(email);

            Assert.Equal(email, user.Email);
            Assert.True(user.EmailConfirmed);
        }

        [Theory]
        [InlineData("userA")]
        [InlineData("userB")]
        public void CreateWithUserName_SetsUserName(string userName)
        {
            var user = TestAppUserIdentityFactory.CreateWithUserName(userName);

            Assert.Equal(userName, user.UserName);
        }
    }
}