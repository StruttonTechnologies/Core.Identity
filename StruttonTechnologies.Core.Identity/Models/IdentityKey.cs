namespace StruttonTechnologies.Core.Identity.Models
{
    /// <summary>
    /// Represents the supported key types for identity entities.
    /// Used to configure identity infrastructure and test scenarios.
    /// </summary>
    public enum IdentityKey
    {
        /// <summary>
        /// Globally unique identifier (GUID) key type.
        /// Recommended for distributed systems and secure identity generation.
        /// </summary>
        Guid,

        /// <summary>
        /// String-based key type.
        /// Commonly used in default ASP.NET Identity setups.
        /// </summary>
        String,

        /// <summary>
        /// Integer-based key type.
        /// Useful for legacy systems or simple identity models.
        /// </summary>
        Int
    }
}
