using ST.Core.Identity.Fakes.Factories;
using System;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Factories
{
    public class TestRefreshTokenFactoryTests
    {
        [Fact]
        public void Create_DefaultValues_ReturnsTokenWithDefaults()
        {
            var token = FakeRefreshTokenFactory.Create();

            Assert.NotNull(token);
            Assert.False(Guid.Empty.Equals(token.UserId));
            Assert.Equal("stub.user", token.Username);
            Assert.False(token.IsRevoked);
            Assert.True(token.ExpiresAt > DateTime.UtcNow);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "alice")]
        [InlineData("22222222-2222-2222-2222-222222222222", "bob")]
        public void Create_WithParameters_SetsUserIdAndUsername(string userIdString, string username)
        {
            var userId = Guid.Parse(userIdString);
            var token = FakeRefreshTokenFactory.Create(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
        }

        [Fact]
        public void Expired_ReturnsTokenWithPastExpiry()
        {
            var token = FakeRefreshTokenFactory.Expired();

            Assert.NotNull(token);
            Assert.True(token.ExpiresAt < DateTime.UtcNow);
            Assert.False(token.IsRevoked);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "charlie")]
        public void Expired_WithParameters_SetsUserIdUsernameAndPastExpiry(string userIdString, string username)
        {
            var userId = Guid.Parse(userIdString);
            var token = FakeRefreshTokenFactory.Expired(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
            Assert.True(token.ExpiresAt < DateTime.UtcNow);
        }

        [Fact]
        public void Revoked_ReturnsTokenWithIsRevokedTrue()
        {
            var token = FakeRefreshTokenFactory.Revoked();

            Assert.NotNull(token);
            Assert.True(token.IsRevoked);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "dave")]
        public void Revoked_WithParameters_SetsUserIdUsernameAndIsRevoked(string userIdString, string username)
        {
            var userId = Guid.Parse(userIdString);
            var token = FakeRefreshTokenFactory.Revoked(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
            Assert.True(token.IsRevoked);
        }
    }
}