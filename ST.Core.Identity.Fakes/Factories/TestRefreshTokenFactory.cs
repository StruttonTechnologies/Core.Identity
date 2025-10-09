using ST.Core.Identity.Stub.Models;

namespace ST.Core.Identity.Fakes.Factories
{
    /// <summary>
    /// Factory for creating test-safe instances of <see cref="StubRefreshToken"/>.
    /// Lives in Fakes for test-only usage, but uses Stub model for realistic shape.
    /// </summary>
    public static class FakeRefreshTokenFactory
    {
        /// <summary>
        /// Creates a valid refresh token with optional user ID and username.
        /// </summary>
        public static StubRefreshToken Create(Guid? userId = null, string? username = null)
        {
            return new StubRefreshToken
            {
                UserId = userId ?? Guid.NewGuid(),
                Username = username ?? "stub.user",
                Token = Guid.NewGuid().ToString("N"),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                CreatedByIp = "127.0.0.1"
            };
        }

        /// <summary>
        /// Creates an expired refresh token for testing expiration logic.
        /// </summary>
        public static StubRefreshToken Expired(Guid? userId = null, string? username = null)
        {
            var token = Create(userId, username);
            token.ExpiresAt = DateTime.UtcNow.AddMinutes(-1);
            return token;
        }

        /// <summary>
        /// Creates a revoked refresh token for testing revocation logic.
        /// </summary>
        public static StubRefreshToken Revoked(Guid? userId = null, string? username = null)
        {
            var token = Create(userId, username);
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedByIp = "127.0.0.1";
            token.ReasonRevoked = "Test scenario";
            return token;
        }
    }
}