using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.Domain.Entities.User;
using ST.Core.Identity.EF.SqlServer.Data;

namespace ST.Core.Identity.EF.SqlServer.Repositories
{
    /// <summary>
    /// Provides SQL Server-backed operations for storing, retrieving, and revoking refresh tokens.
    /// </summary>
    /// <typeparam name="TKey">The type of the user's unique identifier.</typeparam>
    /// <typeparam name="TUser">The type of the user entity.</typeparam>
    /// <typeparam name="TPerson">The type of the person entity.</typeparam>
    public class SqlServerRefreshTokenStore<TKey, TUser, TPerson> : IRefreshTokenStore<TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUserBase<TKey, TPerson>
        where TPerson : PersonBase<TPerson, TKey>   // ✅ fixed constraint
    {
        private readonly SqlServerIdentityDbContext<TKey, TUser, TPerson> _context;

        public SqlServerRefreshTokenStore(SqlServerIdentityDbContext<TKey, TUser, TPerson> context)
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
            var existing = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

            if (existing is null) return;

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RevokeAllAsync(TKey userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _context.RefreshTokens
                .Where(t => t.UserId.Equals(userId) && !t.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}