using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Domain.Entities
{
    /// <summary>
    /// Concrete identity role with customizable key/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the unique identifier for the role.</typeparam>
    [ExcludeFromCodeCoverage]
    public class IdentityRole<TKey> : Microsoft.AspNetCore.Identity.IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the role is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
