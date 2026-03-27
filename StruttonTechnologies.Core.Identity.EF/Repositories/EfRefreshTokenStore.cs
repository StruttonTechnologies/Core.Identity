using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.EF.Repositories
{
    /// <summary>
    /// Provides Entity Framework-backed operations for storing, retrieving, and revoking refresh tokens.
    /// </summary>
    /// <typeparam name="TKey">The type of the user's unique identifier.</typeparam>
    internal class EfRefreshTokenStore<TKey> : IRefreshTokenStore<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly CoreIdentityDbContext<TKey, IdentityUser<TKey>, Microsoft.AspNetCore.Identity.IdentityRole<TKey>> _context;

        public EfRefreshTokenStore(CoreIdentityDbContext<TKey, IdentityUser<TKey>, Microsoft.AspNetCore.Identity.IdentityRole<TKey>> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SaveAsync(RefreshToken<TKey> token, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(token);
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken<TKey>?> GetAsync(string token, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(token);
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked, cancellationToken);
        }

        public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(token);
            RefreshToken<TKey>? existing = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

            if (existing is null)
            {
                return;
            }

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RevokeAllAsync(TKey userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(userId);
            List<RefreshToken<TKey>> tokens = await _context.RefreshTokens
                .Where(t => t.UserId.Equals(userId) && !t.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (RefreshToken<TKey> token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
