namespace StruttonTechnologies.Core.Identity.Domain.Entities.Claim
{
    /// <summary>
    /// Base identity claim with provider metadata.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key for the user associated with the claim.</typeparam>
    public class IdentityClaim<TKey> : Microsoft.AspNetCore.Identity.IdentityUserClaim<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the claim is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets timestamp of claim creation.
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets timestamp of last modification.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
