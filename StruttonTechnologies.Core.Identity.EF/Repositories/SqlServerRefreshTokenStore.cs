using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.EF.Repositories
{
    /// <summary>
    /// Provides SQL Server-backed operations for storing, retrieving, and revoking refresh tokens.
    /// </summary>
    /// <typeparam name="TContext">The Core Identity DbContext type.</typeparam>
    /// <typeparam name="TUser">The identity user type.</typeparam>
    /// <typeparam name="TRole">The identity role type.</typeparam>
    /// <typeparam name="TKey">The type of the user's unique identifier.</typeparam>
    public class SqlServerRefreshTokenStore<TContext, TUser, TRole, TKey> : IRefreshTokenStore<TKey>
        where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
        where TUser : IdentityUser<TKey>
        where TRole : Microsoft.AspNetCore.Identity.IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TContext _context;

        public SqlServerRefreshTokenStore(TContext context)
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
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked, cancellationToken);
        }

        public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

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
            if (EqualityComparer<TKey>.Default.Equals(userId, default))
            {
                throw new ArgumentNullException(nameof(userId));
            }

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
