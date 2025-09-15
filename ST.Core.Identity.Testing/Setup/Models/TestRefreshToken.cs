using System.Diagnostics.CodeAnalysis;
using ST.Core.Identity.Domain.Authentication.Entities;

namespace ST.Core.Identity.Testing.Setup.Models
{
    /// <summary>
    /// Represents a test-safe version of RefreshToken for identity testing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestRefreshToken : RefreshToken
    {
        public TestRefreshToken()
        {
            Token = Guid.NewGuid().ToString("N");
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddDays(7);
            IsRevoked = false;
            UserId = Guid.NewGuid().ToString();
            Username = "test.user";
        }
    }
}


