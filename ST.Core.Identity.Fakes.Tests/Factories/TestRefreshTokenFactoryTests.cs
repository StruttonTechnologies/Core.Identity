using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Fakes.Models;
using System;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Factories
{
    public class TestRefreshTokenFactoryTests
    {
        [Fact]
        public void Create_DefaultValues_ReturnsTokenWithDefaults()
        {
            var token = TestRefreshTokenFactory.Create();

            Assert.NotNull(token);
            Assert.False(string.IsNullOrWhiteSpace(token.UserId));
            Assert.Equal("test.user", token.Username);
            Assert.False(token.IsRevoked);
            Assert.True(token.ExpiresAt > DateTime.UtcNow);
        }

        [Theory]
        [InlineData("user1", "alice")]
        [InlineData("user2", "bob")]
        public void Create_WithParameters_SetsUserIdAndUsername(string userId, string username)
        {
            var token = TestRefreshTokenFactory.Create(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
        }

        [Fact]
        public void Expired_ReturnsTokenWithPastExpiry()
        {
            var token = TestRefreshTokenFactory.Expired();

            Assert.NotNull(token);
            Assert.True(token.ExpiresAt < DateTime.UtcNow);
            Assert.False(token.IsRevoked);
        }

        [Theory]
        [InlineData("user3", "charlie")]
        public void Expired_WithParameters_SetsUserIdUsernameAndPastExpiry(string userId, string username)
        {
            var token = TestRefreshTokenFactory.Expired(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
            Assert.True(token.ExpiresAt < DateTime.UtcNow);
        }

        [Fact]
        public void Revoked_ReturnsTokenWithIsRevokedTrue()
        {
            var token = TestRefreshTokenFactory.Revoked();

            Assert.NotNull(token);
            Assert.True(token.IsRevoked);
        }

        [Theory]
        [InlineData("user4", "dave")]
        public void Revoked_WithParameters_SetsUserIdUsernameAndIsRevoked(string userId, string username)
        {
            var token = TestRefreshTokenFactory.Revoked(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
            Assert.True(token.IsRevoked);
        }
    }
}