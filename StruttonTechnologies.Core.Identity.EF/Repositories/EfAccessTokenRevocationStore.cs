using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.EF.Repositories
{
    /// <summary>
    /// Entity Framework-backed store for access token revocations.
    /// </summary>
    /// <typeparam name="TKey">The type of the user identifier.</typeparam>
    public class EfAccessTokenRevocationStore<TKey> : IAccessTokenRevocationStore<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        private readonly CoreIdentityDbContext<
            TKey,
            StruttonTechnologies.Core.Identity.Domain.Entities.IdentityUser<TKey>,
            StruttonTechnologies.Core.Identity.Domain.Entities.IdentityRole<TKey>> _context;

        public EfAccessTokenRevocationStore(
            CoreIdentityDbContext<
                TKey,
                StruttonTechnologies.Core.Identity.Domain.Entities.IdentityUser<TKey>,
                StruttonTechnologies.Core.Identity.Domain.Entities.IdentityRole<TKey>> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task RevokeAsync(
            string jti,
            TKey userId,
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
            await Task.CompletedTask;
        }
    }
}
