using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Models;
using System.Text;

namespace ST.Core.Identity.Application.Authentication.Factories
{
    /// <summary>
    /// Factory for creating <see cref="JwtTokenOptions"/> instances using configuration settings.
    /// </summary>
    public class TokenOptionsFactory
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenOptionsFactory"/> class.
        /// </summary>
        /// <param name="config">The application configuration.</param>
        public TokenOptionsFactory(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Creates a new <see cref="JwtTokenOptions"/> instance with the specified expiration date.
        /// </summary>
        /// <param name="expirationMinutes">The expiration time in minutes for the token.</param>
        /// <returns>A configured <see cref="JwtTokenOptions"/> instance.</returns>
        public JwtTokenOptions Create(int expirationMinutes)
        {
            var issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer configuration is missing.");
            var audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience configuration is missing.");
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key configuration is missing.");
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256
            );

            return new JwtTokenOptions(issuer, audience, key, creds, expirationMinutes);
        }
    }
}
