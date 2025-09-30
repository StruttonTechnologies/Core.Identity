using ST.Core.Identity.Fakes.Factories;
using ST.Core.Identity.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Factories
{
    public class JwtTokenOptionsFactoryTests
    {
        [Fact]
        public void CreateDefault_ReturnsJwtTokenOptionsWithExpectedValues()
        {
            var options = JwtTokenOptionsFactory.CreateDefault();

            Assert.NotNull(options);
            Assert.Equal("issuer", options.Issuer);
            Assert.Equal("audience", options.Audience);
            Assert.Equal("supersecretkey1234567890supersecretkey", options.Key);
            Assert.Equal(60, options.ExpirationMinutes);
            Assert.NotNull(options.Credentials);
            Assert.IsType<SigningCredentials>(options.Credentials);
            Assert.Equal(SecurityAlgorithms.HmacSha256, options.Credentials.Algorithm);
        }

        [Theory]
        [InlineData("issuer")]
        [InlineData("audience")]
        [InlineData("supersecretkey1234567890supersecretkey")]
        public void CreateDefault_Properties_AreNotNullOrEmpty(string propertyValue)
        {
            var options = JwtTokenOptionsFactory.CreateDefault();

            Assert.False(string.IsNullOrEmpty(propertyValue));
        }

        [Fact]
        public void CreateDefault_CredentialsKeyMatchesOptionsKey()
        {
            var options = JwtTokenOptionsFactory.CreateDefault();
            var credsKey = ((SymmetricSecurityKey)options.Credentials.Key).Key;
            var expectedKey = Encoding.UTF8.GetBytes(options.Key);

            Assert.Equal(expectedKey, credsKey);
        }
    }
}