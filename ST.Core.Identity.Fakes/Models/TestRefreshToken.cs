using System.Diagnostics.CodeAnalysis;
using ST.Core.Identity.Domain.Entities;

namespace ST.Core.Identity.Fakes.Models
{
    /// <summary>
    /// Represents a test-safe version of RefreshToken for identity testing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestRefreshToken : RefreshToken<Guid>
    {
        public TestRefreshToken()
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


