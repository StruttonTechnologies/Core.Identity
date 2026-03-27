namespace StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken
{
    /// <summary>
    /// Persists access-token revocation metadata.
    /// </summary>
    /// <typeparam name="TKey">The type of the user identifier.</typeparam>
    public interface IAccessTokenRevocationStore<TKey>
        where TKey : IEquatable<TKey>
    {
        public Task RevokeAsync(string jti, TKey? userId, DateTime expiresAtUtc, CancellationToken cancellationToken);

        public Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken);

        public Task RevokeAllAsync(TKey userId, CancellationToken cancellationToken);
    }
}
