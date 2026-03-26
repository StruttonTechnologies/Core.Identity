using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.Stub.Models
{
    /// <summary>
    /// Represents a test-safe version of RefreshToken for identity testing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StubRefreshToken : RefreshToken<Guid>
    {
        public StubRefreshToken()
        {
            Token = Guid.NewGuid().ToString("N");
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddDays(7);
            IsRevoked = false;
            UserId = Guid.NewGuid();
            Username = "test.user";
        }
    }
}
