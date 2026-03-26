using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.Fakes.Factories;

namespace StruttonTechnologies.Core.Identity.Fakes.Tests.Factories
{
    public class JwtTokenOptionsFactoryTests
    {
        [Fact]
        public void CreateDefault_ReturnsJwtTokenOptionsWithExpectedValues()
        {
            JwtTokenOptions options = JwtTokenOptionsFactory.CreateDefault();

            Assert.NotNull(options);
            Assert.Equal("issuer", options.Issuer);
            Assert.Equal("audience", options.Audience);
            Assert.Equal("supersecretkey1234567890supersecretkey", options.SigningKey);
            Assert.Equal(60, options.ExpirationMinutes);
        }

        [Theory]
        [InlineData("issuer")]
        [InlineData("audience")]
        [InlineData("supersecretkey1234567890supersecretkey")]
        public void CreateDefault_Properties_AreNotNullOrEmpty(string propertyValue)
        {
            JwtTokenOptions options = JwtTokenOptionsFactory.CreateDefault();

            Assert.False(string.IsNullOrEmpty(propertyValue));
        }
    }
}
