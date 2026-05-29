using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.EF.Repositories
{
    /// <summary>
    /// Entity Framework-backed store for access token revocations.
    /// </summary>
    /// <typeparam name="TContext">The Core Identity DbContext type.</typeparam>
    /// <typeparam name="TUser">The identity user type.</typeparam>
    /// <typeparam name="TRole">The identity role type.</typeparam>
    /// <typeparam name="TKey">The type of the user identifier.</typeparam>
    public class EfAccessTokenRevocationStore<TContext, TUser, TRole, TKey> : IAccessTokenRevocationStore<TKey>
        where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
        where TUser : IdentityUser<TKey>
        where TRole : Microsoft.AspNetCore.Identity.IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TContext _context;

        public EfAccessTokenRevocationStore(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task RevokeAsync(
            string jti,
            TKey? userId,
            DateTime expiresAtUtc,
            CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jti);

            bool exists = await _context.AccessTokenRevocations
                .AnyAsync(x => x.Jti == jti, cancellationToken);

            if (exists)
            {
                return;
            }

            _context.AccessTokenRevocations.Add(new AccessTokenRevocation<TKey>
            {
                Jti = jti,
                UserId = userId,
                ExpiresAtUtc = expiresAtUtc,
                RevokedAtUtc = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jti);

            return _context.AccessTokenRevocations
                .AnyAsync(x => x.Jti == jti, cancellationToken);
        }

        public async Task RevokeAllAsync(TKey userId, CancellationToken cancellationToken)
        {
            if (EqualityComparer<TKey>.Default.Equals(userId, default))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            // Token rows are JTI based, so this records bulk revocation by marking
            // all existing revocation entries for the user. Applications that need
            // full session-wide access token invalidation should issue short-lived
            // access tokens and revoke refresh tokens as the durable session control.
            List<AccessTokenRevocation<TKey>> existingTokens = await _context.AccessTokenRevocations
                .Where(x => x.UserId != null && x.UserId.Equals(userId))
                .ToListAsync(cancellationToken);

            foreach (AccessTokenRevocation<TKey> token in existingTokens)
            {
                token.RevokedAtUtc = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
