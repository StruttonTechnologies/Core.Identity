using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Fakes.Factories;
using StruttonTechnologies.Core.Identity.Stub.Models;

namespace StruttonTechnologies.Core.Identity.Fakes.Tests.Factories
{
    [ExcludeFromCodeCoverage]
    public class TestRefreshTokenFactoryTests
    {
        [Fact]
        public void Create_DefaultValues_ReturnsTokenWithDefaults()
        {
            StubRefreshToken token = FakeRefreshTokenFactory.Create();

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
            Guid userId = Guid.Parse(userIdString);
            StubRefreshToken token = FakeRefreshTokenFactory.Create(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
        }

        [Fact]
        public void Expired_ReturnsTokenWithPastExpiry()
        {
            StubRefreshToken token = FakeRefreshTokenFactory.Expired();

            Assert.NotNull(token);
            Assert.True(token.ExpiresAt < DateTime.UtcNow);
            Assert.False(token.IsRevoked);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "charlie")]
        public void Expired_WithParameters_SetsUserIdUsernameAndPastExpiry(string userIdString, string username)
        {
            Guid userId = Guid.Parse(userIdString);
            StubRefreshToken token = FakeRefreshTokenFactory.Expired(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
            Assert.True(token.ExpiresAt < DateTime.UtcNow);
        }

        [Fact]
        public void Revoked_ReturnsTokenWithIsRevokedTrue()
        {
            StubRefreshToken token = FakeRefreshTokenFactory.Revoked();

            Assert.NotNull(token);
            Assert.True(token.IsRevoked);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "dave")]
        public void Revoked_WithParameters_SetsUserIdUsernameAndIsRevoked(string userIdString, string username)
        {
            Guid userId = Guid.Parse(userIdString);
            StubRefreshToken token = FakeRefreshTokenFactory.Revoked(userId, username);

            Assert.Equal(userId, token.UserId);
            Assert.Equal(username, token.Username);
            Assert.True(token.IsRevoked);
        }
    }
}
