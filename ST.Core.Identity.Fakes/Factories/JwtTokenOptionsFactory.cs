using System.Text;
using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Models;

namespace ST.Core.Identity.Fakes.Factories
{
    /// <summary>
    /// Factory for creating test-safe <see cref="JwtTokenOptions"/> instances.
    /// Used to configure token generation logic in unit and integration tests.
    /// </summary>
    public static class JwtTokenOptionsFactory
    {
        /// <summary>
        /// Creates a default <see cref="JwtTokenOptions"/> instance with hardcoded values.
        /// Suitable for most token generation tests.
        /// </summary>
        /// <returns>A configured <see cref="JwtTokenOptions"/> object.</returns>
        public static JwtTokenOptions CreateDefault()
        {
            var key = "supersecretkey1234567890supersecretkey";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            return new JwtTokenOptions(
                issuer: "issuer",
                audience: "audience",
                key: key,
                credentials: creds,
                expirationMinutes: 60
            );
        }
    }
}
