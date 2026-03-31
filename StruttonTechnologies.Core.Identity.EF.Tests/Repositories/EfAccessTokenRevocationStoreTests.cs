using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

using StruttonTechnologies.Core.Identity.EF.Repositories;

using DomainIdentityRole = StruttonTechnologies.Core.Identity.Domain.Entities.IdentityRole<System.Guid>;
using DomainIdentityUser = StruttonTechnologies.Core.Identity.Domain.Entities.IdentityUser<System.Guid>;
using DomainAccessTokenRevocation = StruttonTechnologies.Core.Identity.Domain.Entities.AccessTokenRevocation<System.Guid>;

namespace StruttonTechnologies.Core.Identity.EF.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class EfAccessTokenRevocationStoreTests : IDisposable
    {
        private readonly CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole> _context;
        private readonly EfAccessTokenRevocationStore<Guid> _store;

        public EfAccessTokenRevocationStoreTests()
        {
            DbContextOptions<CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole>> options =
                new DbContextOptionsBuilder<CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole>>()
                    .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
                    .Options;

            _context = new CoreIdentityDbContext<Guid, DomainIdentityUser, DomainIdentityRole>(options);
            _store = new EfAccessTokenRevocationStore<Guid>(_context);
        }

        [Fact]
        public async Task RevokeAsync_AddsRevocationRecord()
        {
            string jti = Guid.NewGuid().ToString();
            Guid userId = Guid.NewGuid();
            DateTime expiresAt = DateTime.UtcNow.AddHours(1);

            await _store.RevokeAsync(jti, userId, expiresAt, CancellationToken.None);

            DomainAccessTokenRevocation? revocation = await _context.AccessTokenRevocations
                .FirstOrDefaultAsync(r => r.Jti == jti);

            Assert.NotNull(revocation);
            Assert.Equal(jti, revocation.Jti);
            Assert.Equal(userId, revocation.UserId);
            Assert.Equal(expiresAt, revocation.ExpiresAtUtc);
            Assert.True(revocation.RevokedAtUtc <= DateTime.UtcNow);
        }

        [Fact]
        public async Task RevokeAsync_DoesNotAddDuplicate_WhenJtiAlreadyExists()
        {
            string jti = Guid.NewGuid().ToString();
            Guid userId = Guid.NewGuid();
            DateTime expiresAt = DateTime.UtcNow.AddHours(1);

            await _store.RevokeAsync(jti, userId, expiresAt, CancellationToken.None);
            await _store.RevokeAsync(jti, userId, expiresAt, CancellationToken.None);

            int count = await _context.AccessTokenRevocations.CountAsync(r => r.Jti == jti);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RevokeAsync_ThrowsArgumentNullException_WhenJtiIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _store.RevokeAsync(null!, Guid.NewGuid(), DateTime.UtcNow, CancellationToken.None));
        }

        [Fact]
        public async Task RevokeAsync_ThrowsArgumentException_WhenJtiIsEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _store.RevokeAsync(string.Empty, Guid.NewGuid(), DateTime.UtcNow, CancellationToken.None));
        }

        [Fact]
        public async Task RevokeAsync_ThrowsArgumentException_WhenJtiIsWhitespace()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _store.RevokeAsync("   ", Guid.NewGuid(), DateTime.UtcNow, CancellationToken.None));
        }

        [Fact]
        public async Task IsRevokedAsync_ReturnsTrue_WhenJtiIsRevoked()
        {
            string jti = Guid.NewGuid().ToString();
            await _store.RevokeAsync(jti, Guid.NewGuid(), DateTime.UtcNow.AddHours(1), CancellationToken.None);

            bool isRevoked = await _store.IsRevokedAsync(jti, CancellationToken.None);

            Assert.True(isRevoked);
        }

        [Fact]
        public async Task IsRevokedAsync_ReturnsFalse_WhenJtiIsNotRevoked()
        {
            string jti = Guid.NewGuid().ToString();

            bool isRevoked = await _store.IsRevokedAsync(jti, CancellationToken.None);

            Assert.False(isRevoked);
        }

        [Fact]
        public async Task IsRevokedAsync_ThrowsArgumentNullException_WhenJtiIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _store.IsRevokedAsync(null!, CancellationToken.None));
        }

        [Fact]
        public async Task IsRevokedAsync_ThrowsArgumentException_WhenJtiIsEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _store.IsRevokedAsync(string.Empty, CancellationToken.None));
        }

        [Fact]
        public async Task IsRevokedAsync_ThrowsArgumentException_WhenJtiIsWhitespace()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _store.IsRevokedAsync("   ", CancellationToken.None));
        }

        [Fact]
        public async Task RevokeAllAsync_CompletesSuccessfully()
        {
            Guid userId = Guid.NewGuid();

            await _store.RevokeAllAsync(userId, CancellationToken.None);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new EfAccessTokenRevocationStore<Guid>(null!));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
