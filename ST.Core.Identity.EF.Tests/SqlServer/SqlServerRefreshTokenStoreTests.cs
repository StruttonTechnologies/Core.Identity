using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.Domain.Entities.User;
using ST.Core.Identity.EF.SqlServer.Data;
using ST.Core.Identity.EF.SqlServer.Repositories;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ST.Core.Identity.EF.SqlServer.Tests.Repositories
{
    public class SqlServerRefreshTokenStoreTests
    {


        private class TestUser : IdentityUserBase<Guid, TestAppPerson> { }

        private SqlServerIdentityDbContext<Guid, TestUser, TestAppPerson> CreateContext()
        {
            var options = new DbContextOptionsBuilder<SqlServerIdentityDbContext<Guid, TestUser, TestAppPerson>>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new SqlServerIdentityDbContext<Guid, TestUser, TestAppPerson>(options);
        }

        private SqlServerRefreshTokenStore<Guid, TestUser, TestAppPerson> CreateStore(SqlServerIdentityDbContext<Guid, TestUser, TestAppPerson> context)
        {
            return new SqlServerRefreshTokenStore<Guid, TestUser, TestAppPerson>(context);
        }

        [Fact]
        public async Task SaveAsync_PersistsToken()
        {
            var context = CreateContext();
            var store = CreateStore(context);

            var token = new RefreshToken<Guid>
            {
                Token = "abc123",
                UserId = Guid.NewGuid(),
                Username = "testuser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await store.SaveAsync(token);

            var saved = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == "abc123");
            Assert.NotNull(saved);
            Assert.Equal("testuser", saved.Username);
        }

        [Fact]
        public async Task GetAsync_ReturnsToken_IfNotRevoked()
        {
            var context = CreateContext();
            var store = CreateStore(context);

            var token = new RefreshToken<Guid>
            {
                Token = "xyz789",
                UserId = Guid.NewGuid(),
                Username = "activeuser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            context.RefreshTokens.Add(token);
            await context.SaveChangesAsync();

            var result = await store.GetAsync("xyz789");
            Assert.NotNull(result);
            Assert.Equal("activeuser", result!.Username);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_IfRevoked()
        {
            var context = CreateContext();
            var store = CreateStore(context);

            var token = new RefreshToken<Guid>
            {
                Token = "revoked123",
                UserId = Guid.NewGuid(),
                Username = "revokeduser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = true
            };

            context.RefreshTokens.Add(token);
            await context.SaveChangesAsync();

            var result = await store.GetAsync("revoked123");
            Assert.Null(result);
        }

        [Fact]
        public async Task RevokeAsync_SetsIsRevokedTrue()
        {
            var context = CreateContext();
            var store = CreateStore(context);

            var token = new RefreshToken<Guid>
            {
                Token = "revoke-me",
                UserId = Guid.NewGuid(),
                Username = "user",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            context.RefreshTokens.Add(token);
            await context.SaveChangesAsync();

            await store.RevokeAsync("revoke-me");

            var updated = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == "revoke-me");
            Assert.True(updated!.IsRevoked);
            Assert.NotNull(updated.RevokedAt);
        }

        [Fact]
        public async Task RevokeAllAsync_RevokesAllTokensForUser()
        {
            var context = CreateContext();
            var store = CreateStore(context);

            var userId = Guid.NewGuid();

            context.RefreshTokens.AddRange(
                new RefreshToken<Guid> { Token = "t1", UserId = userId, Username = "u", CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(7), IsRevoked = false },
                new RefreshToken<Guid> { Token = "t2", UserId = userId, Username = "u", CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(7), IsRevoked = false }
            );

            await context.SaveChangesAsync();

            await store.RevokeAllAsync(userId);

            var tokens = await context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            Assert.All(tokens, t => Assert.True(t.IsRevoked));
        }
    }
}