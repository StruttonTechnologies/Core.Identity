using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace StruttonTechnologies.Core.Identity.Fakes.Builders
{
    /// <summary>
    /// Builder for creating test-safe <see cref="ClaimsPrincipal"/> instances.
    /// Used to simulate authenticated users in token and authorization tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TestClaimsPrincipalBuilder
    {
        /// <summary>
        /// Creates a default <see cref="ClaimsPrincipal"/> with common identity claims.
        /// Includes NameIdentifier, Name, and Role ("Admin").
        /// </summary>
        /// <returns>A configured <see cref="ClaimsPrincipal"/> object.</returns>
        public static ClaimsPrincipal CreateDefault()
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, "StubUser"),
                new(ClaimTypes.Role, "Admin")
            ];

            ClaimsIdentity identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }
    }
}
