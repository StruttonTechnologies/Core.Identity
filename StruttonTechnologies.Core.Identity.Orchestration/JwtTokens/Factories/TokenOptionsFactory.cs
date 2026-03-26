using Microsoft.Extensions.Configuration;

using StruttonTechnologies.Core.Identity.Domain.Models;

namespace StruttonTechnologies.Core.Identity.Orchestration.JwtTokens.Factories
{
    /// <summary>
    /// Factory for creating <see cref="JwtTokenOptions"/> instances using configuration settings.
    /// </summary>
    public class TokenOptionsFactory
    {
        private readonly IConfiguration _config;

        public TokenOptionsFactory(IConfiguration config)
        {
            _config = config;
        }

        public JwtTokenOptions Create(int expirationMinutes)
        {
            string issuer = _config["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Jwt:Issuer configuration is missing.");
            string audience = _config["Jwt:Audience"]
                ?? throw new InvalidOperationException("Jwt:Audience configuration is missing.");
            string key = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key configuration is missing.");

            return new JwtTokenOptions
            {
                Issuer = issuer,
                Audience = audience,
                SigningKey = key,
                ExpirationMinutes = expirationMinutes
            };
        }
    }
}
