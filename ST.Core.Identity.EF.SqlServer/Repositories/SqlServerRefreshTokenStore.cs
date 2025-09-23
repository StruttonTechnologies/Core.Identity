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
        where TPerson : PersonBase<TPerson>
    {
        private readonly SqlServerIdentityDbContext<TKey, TUser, TPerson> _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerRefreshTokenStore{TKey, TUser, TPerson}"/> class.
        /// </summary>
        /// <param name="context">The SQL Server identity database context.</param>
        public SqlServerRefreshTokenStore(SqlServerIdentityDbContext<TKey, TUser, TPerson> context)
        {
            _context = context;
        }

        /// <summary>
        /// Saves a refresh token to the database.
        /// </summary>
        /// <param name="token">The refresh token to persist.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        public async Task SaveAsync(RefreshToken<TKey> token, CancellationToken cancellationToken = default)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a refresh token by its token string, if it exists and is not revoked.
        /// </summary>
        /// <param name="token">The token string to search for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The matching refresh token, or null if not found or revoked.</returns>
        public async Task<RefreshToken<TKey>?> GetAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked, cancellationToken);
        }

        /// <summary>
        /// Revokes a specific refresh token by its token string.
        /// </summary>
        /// <param name="token">The token string to revoke.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
        {
            var existing = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

            if (existing is null) return;

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Revokes all active refresh tokens associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
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