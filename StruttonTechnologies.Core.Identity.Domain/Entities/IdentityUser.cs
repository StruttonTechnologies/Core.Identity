using System.ComponentModel.DataAnnotations;

namespace StruttonTechnologies.Core.Identity.Domain.Entities
{
    /// <summary>
    /// Concrete identity user with customizable key and person types.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key for the user.</typeparam>
    public class IdentityUser<TKey> : Microsoft.AspNetCore.Identity.IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the name of the identity provider (e.g., Google, Facebook, Local).
        /// </summary>
        public string? ProviderName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the user is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets used for optimistic concurrency control.
        /// </summary>
        [ConcurrencyCheck]
        public int RowVersion { get; set; }

        /// <summary>
        /// Gets or sets timestamp of user creation.
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets timestamp of last modification.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
