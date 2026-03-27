namespace StruttonTechnologies.Core.Identity.Domain.Entities
{
    /// <summary>
    /// Represents a revoked access token JTI persisted for revocation checks.
    /// </summary>
    /// <typeparam name="TKey">The type of the user identifier.</typeparam>
    public class AccessTokenRevocation<TKey>
        where TKey : IEquatable<TKey>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Jti { get; set; } = string.Empty;

        public TKey? UserId { get; set; }

        public DateTime ExpiresAtUtc { get; set; }

        public DateTime RevokedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
