using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.JwtTokenManager.Configuration;

namespace StruttonTechnologies.Core.Identity.Infrastructure.Tests.Configuration
{
    /// <summary>
    /// Contains test scenarios for <see cref="TokenProviderOptionsConfig"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TokenProviderOptionsConfigTests
    {
        [Fact]
        public void Constructor_DefaultsTokenLifespanHoursTo24()
        {
            TokenProviderOptionsConfig options = new();

            Assert.Equal(24, options.TokenLifespanHours);
        }

        [Fact]
        public void TokenLifespanHours_CanBeSet()
        {
            TokenProviderOptionsConfig options = new()
            {
                TokenLifespanHours = 48
            };

            Assert.Equal(48, options.TokenLifespanHours);
        }

        [Fact]
        public void TokenLifespanHours_CanBeRetrieved()
        {
            TokenProviderOptionsConfig options = new();
            options.TokenLifespanHours = 72;

            int result = options.TokenLifespanHours;

            Assert.Equal(72, result);
        }
    }
}
