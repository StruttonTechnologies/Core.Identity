using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Domain.Interfaces.Jwtoken;

namespace StruttonTechnologies.Core.Identity.EF.Repositories
{
    /// <summary>
    /// Provides SQL Server-backed operations for storing, retrieving, and revoking refresh tokens.
    /// </summary>
    /// <typeparam name="TKey">The type of the user's unique identifier.</typeparam>
    public class SqlServerRefreshTokenStore<TKey> : IRefreshTokenStore<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly CoreIdentityDbContext<TKey, IdentityUser<TKey>, Microsoft.AspNetCore.Identity.IdentityRole<TKey>> _context;

        public SqlServerRefreshTokenStore(CoreIdentityDbContext<TKey, IdentityUser<TKey>, Microsoft.AspNetCore.Identity.IdentityRole<TKey>> context)
        {
            _context = context;
        }

        public async Task SaveAsync(RefreshToken<TKey> token, CancellationToken cancellationToken = default)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken<TKey>?> GetAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked, cancellationToken);
        }

        public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
        {
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
            List<RefreshToken<TKey>> tokens = await _context.RefreshTokens
                .Where(t => t.UserId.Equals(userId) && !t.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (RefreshToken<TKey>? token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
