using ST.Core.Identity.Fakes.Models;

namespace ST.Core.Identity.Fakes.Factories
{
    /// <summary>
    /// Factory for creating test-safe instances of TestRefreshToken.
    /// </summary>
    public static class TestRefreshTokenFactory
    {
        public static TestRefreshToken Create(string? userId = null, string? username = null)
        {
            return new TestRefreshToken
            {
                UserId = userId ?? Guid.NewGuid().ToString(),
                Username = username ?? "test.user"
            };
        }

        public static TestRefreshToken Expired(string? userId = null, string? username = null)
        {
            var token = Create(userId, username);
            token.ExpiresAt = DateTime.UtcNow.AddMinutes(-1);
            return token;
        }

        public static TestRefreshToken Revoked(string? userId = null, string? username = null)
        {
            var token = Create(userId, username);
            token.IsRevoked = true;
            return token;
        }
    }
}