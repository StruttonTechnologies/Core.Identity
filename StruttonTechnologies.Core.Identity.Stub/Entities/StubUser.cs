using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Stub.Entities
{
    /// <summary>
    /// Stub user entity for testing and mocking Identity flows.
    /// Inherits from built-in IdentityUser with Guid key.
    /// </summary>
    public sealed class StubUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Optional display name for test scenarios.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Optional flag to simulate locked-out users.
        /// </summary>
        public bool IsLockedOut { get; set; }

        /// <summary>
        /// Optional flag to simulate confirmed email.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// In-memory role list for testing IUserRoleStore.
        /// Not present in real IdentityUser; simulated for test scenarios.
        /// </summary>
        public HashSet<string> Roles { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Optional override for ToString in test logs.
        /// </summary>
        public override string ToString()
        {
            return DisplayName ?? UserName ?? Email ?? base.ToString();
        }
    }
}
