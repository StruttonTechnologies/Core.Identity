using StruttonTechnologies.Core.Identity.JwtTokenManager.Configuration;

namespace StruttonTechnologies.Core.Identity.Infrastructure.Tests.Configuration
{
    /// <summary>
    /// Contains test scenarios for <see cref="TokenProviderOptionsConfig"/>.
    /// </summary>
    public class TokenProviderOptionsConfigTests
    {
        [Fact]
        public void Constructor_DefaultsTokenLifespanHoursTo24()
        {
            TokenProviderOptionsConfig options = new();

            Assert.Equal(24, options.TokenLifespanHours);
        }
    }
}
