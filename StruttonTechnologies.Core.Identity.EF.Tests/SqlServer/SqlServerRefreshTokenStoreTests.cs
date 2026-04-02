using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

using StruttonTechnologies.Core.Identity.EF.Repositories;

using DomainIdentityRole = Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>;
using DomainIdentityUser = StruttonTechnologies.Core.Identity.Domain.Entities.IdentityUser<System.Guid>;
using DomainRefreshToken = StruttonTechnologies.Core.Identity.Domain.Entities.RefreshToken<System.Guid>;

namespace StruttonTechnologies.Core.Identity.EF.Tests.SqlServer
{
    /// <summary>
    /// Contains test scenarios for <see cref="SqlServerRefreshTokenStore{TKey}"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SqlServerRefreshTokenStoreTests : IDisposable
    {
        private readonly CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole> _context;
        private readonly SqlServerRefreshTokenStore<Guid> _store;

        public SqlServerRefreshTokenStoreTests()
        {
            DbContextOptions<CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole>> options =
                new DbContextOptionsBuilder<CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole>>()
                    .UseInMemoryDatabase($"SqlServerTestDb_{Guid.NewGuid()}")
                    .Options;

            _context = new CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole>(options);
            _store = new SqlServerRefreshTokenStore<Guid>(_context);
        }

        [Fact]
        public async Task SaveAsync_AddsTokenToDatabase()
        {
            DomainRefreshToken token = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            await _store.SaveAsync(token, CancellationToken.None);

            DomainRefreshToken? savedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token.Token);

            Assert.NotNull(savedToken);
            Assert.Equal(token.Token, savedToken.Token);
            Assert.Equal(token.UserId, savedToken.UserId);
            Assert.Equal(token.Username, savedToken.Username);
        }

        [Fact]
        public async Task GetAsync_ReturnsToken_WhenTokenExists()
        {
            DomainRefreshToken token = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            await _store.SaveAsync(token, CancellationToken.None);

            DomainRefreshToken? retrievedToken = await _store.GetAsync(token.Token, CancellationToken.None);

            Assert.NotNull(retrievedToken);
            Assert.Equal(token.Token, retrievedToken.Token);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenTokenDoesNotExist()
        {
            DomainRefreshToken? token = await _store.GetAsync("nonexistent", CancellationToken.None);

            Assert.Null(token);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenTokenIsRevoked()
        {
            DomainRefreshToken token = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = true,
                RevokedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            DomainRefreshToken? retrievedToken = await _store.GetAsync(token.Token, CancellationToken.None);

            Assert.Null(retrievedToken);
        }

        [Fact]
        public async Task RevokeAsync_MarksTokenAsRevoked()
        {
            DomainRefreshToken token = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            await _store.SaveAsync(token, CancellationToken.None);
            await _store.RevokeAsync(token.Token, CancellationToken.None);

            DomainRefreshToken? revokedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token.Token);

            Assert.NotNull(revokedToken);
            Assert.True(revokedToken.IsRevoked);
            Assert.NotNull(revokedToken.RevokedAt);
        }

        [Fact]
        public async Task RevokeAsync_DoesNotThrow_WhenTokenDoesNotExist()
        {
            await _store.RevokeAsync("nonexistent", CancellationToken.None);
        }

        [Fact]
        public async Task RevokeAllAsync_RevokesAllTokensForUser()
        {
            Guid userId = Guid.NewGuid();

            DomainRefreshToken token1 = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            DomainRefreshToken token2 = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            await _store.SaveAsync(token1, CancellationToken.None);
            await _store.SaveAsync(token2, CancellationToken.None);

            await _store.RevokeAllAsync(userId, CancellationToken.None);

            List<DomainRefreshToken> tokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            Assert.All(tokens, t => Assert.True(t.IsRevoked));
            Assert.All(tokens, t => Assert.NotNull(t.RevokedAt));
        }

        [Fact]
        public async Task RevokeAllAsync_DoesNotAffectOtherUsers()
        {
            Guid userId1 = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();

            DomainRefreshToken token1 = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId1,
                Username = "User1",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            DomainRefreshToken token2 = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId2,
                Username = "User2",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            await _store.SaveAsync(token1, CancellationToken.None);
            await _store.SaveAsync(token2, CancellationToken.None);

            await _store.RevokeAllAsync(userId1, CancellationToken.None);

            DomainRefreshToken? user1Token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.UserId == userId1);
            DomainRefreshToken? user2Token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.UserId == userId2);

            Assert.NotNull(user1Token);
            Assert.True(user1Token.IsRevoked);

            Assert.NotNull(user2Token);
            Assert.False(user2Token.IsRevoked);
        }

        [Fact]
        public async Task RevokeAllAsync_DoesNotRevokeAlreadyRevokedTokens()
        {
            Guid userId = Guid.NewGuid();
            DateTime originalRevokedAt = DateTime.UtcNow.AddHours(-1);

            DomainRefreshToken alreadyRevokedToken = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = true,
                RevokedAt = originalRevokedAt
            };

            DomainRefreshToken activeToken = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(alreadyRevokedToken);
            await _store.SaveAsync(activeToken, CancellationToken.None);

            await _store.RevokeAllAsync(userId, CancellationToken.None);

            DomainRefreshToken? token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == alreadyRevokedToken.Token);

            Assert.NotNull(token);
            Assert.Equal(originalRevokedAt, token.RevokedAt);
        }

        [Fact]
        public async Task SaveAsync_WithCancellationToken_CancelsOperation()
        {
            DomainRefreshToken token = new()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Username = "TestUser",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            CancellationTokenSource cts = new();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _store.SaveAsync(token, cts.Token));
        }

        [Fact]
        public async Task GetAsync_WithCancellationToken_CancelsOperation()
        {
            CancellationTokenSource cts = new();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _store.GetAsync("test-token", cts.Token));
        }

        [Fact]
        public async Task RevokeAsync_WithCancellationToken_CancelsOperation()
        {
            CancellationTokenSource cts = new();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _store.RevokeAsync("test-token", cts.Token));
        }

        [Fact]
        public async Task RevokeAllAsync_WithCancellationToken_CancelsOperation()
        {
            CancellationTokenSource cts = new();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _store.RevokeAllAsync(Guid.NewGuid(), cts.Token));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
