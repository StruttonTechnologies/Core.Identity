using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Testing.Setup.Factories;
using System.Text;
using Xunit;

namespace ST.Core.Identity.Testing.Tests.Setup.Factories
{
    /// <summary>
    /// Unit tests for <see cref="JwtTokenOptionsFactory"/>.
    /// Validates default token configuration and signing credentials.
    /// </summary>
    public class JwtTokenOptionsFactoryTests
    {
        [Fact]
        public void CreateDefault_ReturnsValidOptions()
        {
            var options = JwtTokenOptionsFactory.CreateDefault();

            Assert.NotNull(options);
            Assert.Equal("issuer", options.Issuer);
            Assert.Equal("audience", options.Audience);
            Assert.Equal(60, options.ExpirationMinutes);
            Assert.NotNull(options.Key);
            Assert.NotNull(options.Credentials);

            var expectedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            Assert.Equal(SecurityAlgorithms.HmacSha256, options.Credentials.Algorithm);
        }
    }
}