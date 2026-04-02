using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;

using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.Orchestration.JwtTokens.Factories;

namespace StruttonTechnologies.Core.Identity.Orchestration.Tests.Factories
{
    [ExcludeFromCodeCoverage]
    public class TokenOptionsFactoryTests
    {
        [Fact]
        public void Create_ReturnsValidOptions_WhenAllConfigurationIsPresent()
        {
            var configValues = new Dictionary<string, string>
            {
                { "Jwt:Issuer", "test-issuer" },
                { "Jwt:Audience", "test-audience" },
                { "Jwt:Key", "test-signing-key-that-is-at-least-32-characters-long" }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues!)
                .Build();

            TokenOptionsFactory factory = new(config);
            int expirationMinutes = 60;

            JwtTokenOptions result = factory.Create(expirationMinutes);

            Assert.NotNull(result);
            Assert.Equal("test-issuer", result.Issuer);
            Assert.Equal("test-audience", result.Audience);
            Assert.Equal("test-signing-key-that-is-at-least-32-characters-long", result.SigningKey);
            Assert.Equal(60, result.ExpirationMinutes);
        }

        [Fact]
        public void Create_ThrowsInvalidOperationException_WhenIssuerIsMissing()
        {
            var configValues = new Dictionary<string, string>
            {
                { "Jwt:Audience", "test-audience" },
                { "Jwt:Key", "test-key" }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues!)
                .Build();

            TokenOptionsFactory factory = new(config);

            var ex = Assert.Throws<InvalidOperationException>(() => factory.Create(60));
            Assert.Contains("Jwt:Issuer", ex.Message);
        }

        [Fact]
        public void Create_ThrowsInvalidOperationException_WhenAudienceIsMissing()
        {
            var configValues = new Dictionary<string, string>
            {
                { "Jwt:Issuer", "test-issuer" },
                { "Jwt:Key", "test-key" }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues!)
                .Build();

            TokenOptionsFactory factory = new(config);

            var ex = Assert.Throws<InvalidOperationException>(() => factory.Create(60));
            Assert.Contains("Jwt:Audience", ex.Message);
        }

        [Fact]
        public void Create_ThrowsInvalidOperationException_WhenKeyIsMissing()
        {
            var configValues = new Dictionary<string, string>
            {
                { "Jwt:Issuer", "test-issuer" },
                { "Jwt:Audience", "test-audience" }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues!)
                .Build();

            TokenOptionsFactory factory = new(config);

            var ex = Assert.Throws<InvalidOperationException>(() => factory.Create(60));
            Assert.Contains("Jwt:Key", ex.Message);
        }

        [Fact]
        public void Create_UsesCustomExpirationMinutes()
        {
            var configValues = new Dictionary<string, string>
            {
                { "Jwt:Issuer", "test-issuer" },
                { "Jwt:Audience", "test-audience" },
                { "Jwt:Key", "test-key" }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues!)
                .Build();

            TokenOptionsFactory factory = new(config);

            JwtTokenOptions result = factory.Create(120);

            Assert.Equal(120, result.ExpirationMinutes);
        }
    }
}
