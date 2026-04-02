using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Domain.Models;

namespace StruttonTechnologies.Core.Identity.Stub.Factories
{
    /// <summary>
    /// Factory for creating test-safe <see cref="JwtTokenOptions"/> instances.
    /// Used to configure token generation logic in unit and integration tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class StubJwtOptionsFactory
    {
        /// <summary>
        /// Creates a default <see cref="JwtTokenOptions"/> instance with hardcoded values.
        /// Suitable for most token generation tests.
        /// </summary>
        /// <returns>A configured <see cref="JwtTokenOptions"/> object.</returns>
        public static JwtTokenOptions CreateDefault()
        {
            return new JwtTokenOptions
            {
                Issuer = "issuer",
                Audience = "audience",
                SigningKey = "supersecretkey1234567890supersecretkey",
                ExpirationMinutes = 60
            };
        }
    }
}
