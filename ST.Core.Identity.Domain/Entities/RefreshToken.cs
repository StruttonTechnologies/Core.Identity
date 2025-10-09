using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Core.Identity.Domain.Entities
{
    /// <summary>
    /// Represents a refresh token used for renewing authentication sessions.
    /// </summary>
    public class RefreshToken<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Surrogate primary key for EF and indexing.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The refresh token string.
        /// </summary>
        public string Token { get; set; } = default!;

        /// <summary>
        /// The identifier of the user associated with this token.
        /// </summary>
        public TKey UserId { get; set; } = default!;

        /// <summary>
        /// The username of the user associated with this token.
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
        /// When the token was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// When the token was revoked, if applicable.
        /// </summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>
        /// Whether the token has been revoked.
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        /// IP address where the token was created.
        /// </summary>
        public string? CreatedByIp { get; set; }

        /// <summary>
        /// IP address where the token was revoked.
        /// </summary>
        public string? RevokedByIp { get; set; }

        /// <summary>
        /// Token that replaced this one (if rotated).
        /// </summary>
        public string? ReplacedByToken { get; set; }

        /// <summary>
        /// Reason for revocation.
        /// </summary>
        public string? ReasonRevoked { get; set; }

        /// <summary>
        /// Convenience property to check if the token is still valid.
        /// Not mapped to the database.
        /// </summary>
        public bool IsActive => !IsRevoked && DateTime.UtcNow <= ExpiresAt;
    }
}