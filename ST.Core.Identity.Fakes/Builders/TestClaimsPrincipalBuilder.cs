using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ST.Core.Identity.Fakes.Builders
{
    /// <summary>
    /// Builder for creating test-safe <see cref="ClaimsPrincipal"/> instances.
    /// Used to simulate authenticated users in token and authorization tests.
    /// </summary>
    public static class TestClaimsPrincipalBuilder
    {
        /// <summary>
        /// Creates a default <see cref="ClaimsPrincipal"/> with common identity claims.
        /// Includes NameIdentifier, Name, and Role ("Admin").
        /// </summary>
        /// <returns>A configured <see cref="ClaimsPrincipal"/> object.</returns>
        public static ClaimsPrincipal CreateDefault()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, "testuser"),
                new(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }
    }
}
