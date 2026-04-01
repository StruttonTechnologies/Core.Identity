using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Tests.Data
{
    [ExcludeFromCodeCoverage]
    public class KnownIdentityProvidersTests
    {
        [Fact]
        public void Google_HasCorrectValue()
        {
            Assert.Equal("Google", KnownIdentityProviders.Google);
        }

        [Fact]
        public void Microsoft_HasCorrectValue()
        {
            Assert.Equal("Microsoft", KnownIdentityProviders.Microsoft);
        }

        [Fact]
        public void GitHub_HasCorrectValue()
        {
            Assert.Equal("GitHub", KnownIdentityProviders.GitHub);
        }

        [Fact]
        public void Okta_HasCorrectValue()
        {
            Assert.Equal("Okta", KnownIdentityProviders.Okta);
        }

        [Fact]
        public void Auth0_HasCorrectValue()
        {
            Assert.Equal("Auth0", KnownIdentityProviders.Auth0);
        }

        [Fact]
        public void Local_HasCorrectValue()
        {
            Assert.Equal("Local", KnownIdentityProviders.Local);
        }

        [Fact]
        public void All_ContainsAllProviders()
        {
            string[] providers = KnownIdentityProviders.All;

            Assert.Contains(KnownIdentityProviders.Google, providers);
            Assert.Contains(KnownIdentityProviders.Microsoft, providers);
            Assert.Contains(KnownIdentityProviders.GitHub, providers);
            Assert.Contains(KnownIdentityProviders.Okta, providers);
            Assert.Contains(KnownIdentityProviders.Auth0, providers);
            Assert.Contains(KnownIdentityProviders.Local, providers);
        }

        [Fact]
        public void All_HasCorrectCount()
        {
            string[] providers = KnownIdentityProviders.All;

            Assert.Equal(6, providers.Length);
        }

        [Fact]
        public void All_HasCorrectOrder()
        {
            string[] providers = KnownIdentityProviders.All;

            Assert.Equal(KnownIdentityProviders.Google, providers[0]);
            Assert.Equal(KnownIdentityProviders.Microsoft, providers[1]);
            Assert.Equal(KnownIdentityProviders.GitHub, providers[2]);
            Assert.Equal(KnownIdentityProviders.Okta, providers[3]);
            Assert.Equal(KnownIdentityProviders.Auth0, providers[4]);
            Assert.Equal(KnownIdentityProviders.Local, providers[5]);
        }

        [Fact]
        public void First_WithDefaultParameter_ReturnsGoogleProvider()
        {
            string[] result = KnownIdentityProviders.First();

            Assert.Single(result);
            Assert.Equal(KnownIdentityProviders.Google, result[0]);
        }

        [Fact]
        public void First_WithCount3_ReturnsFirstThreeProviders()
        {
            string[] result = KnownIdentityProviders.First(3);

            Assert.Equal(3, result.Length);
            Assert.Equal(KnownIdentityProviders.Google, result[0]);
            Assert.Equal(KnownIdentityProviders.Microsoft, result[1]);
            Assert.Equal(KnownIdentityProviders.GitHub, result[2]);
        }

        [Fact]
        public void First_WithZeroCount_ReturnsOneProvider()
        {
            string[] result = KnownIdentityProviders.First(0);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithNegativeCount_ReturnsOneProvider()
        {
            string[] result = KnownIdentityProviders.First(-1);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithCountGreaterThanTotal_ReturnsAllProviders()
        {
            string[] result = KnownIdentityProviders.First(100);

            Assert.Equal(6, result.Length);
        }

        [Fact]
        public void Last_WithDefaultParameter_ReturnsLocalProvider()
        {
            string[] result = KnownIdentityProviders.Last();

            Assert.Single(result);
            Assert.Equal(KnownIdentityProviders.Local, result[0]);
        }

        [Fact]
        public void Last_WithCount3_ReturnsLastThreeProviders()
        {
            string[] result = KnownIdentityProviders.Last(3);

            Assert.Equal(3, result.Length);
            Assert.Equal(KnownIdentityProviders.Okta, result[0]);
            Assert.Equal(KnownIdentityProviders.Auth0, result[1]);
            Assert.Equal(KnownIdentityProviders.Local, result[2]);
        }

        [Fact]
        public void Last_WithZeroCount_ReturnsEmptyArray()
        {
            string[] result = KnownIdentityProviders.Last(0);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithNegativeCount_ReturnsEmptyArray()
        {
            string[] result = KnownIdentityProviders.Last(-1);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithCountGreaterThanTotal_ReturnsAllProviders()
        {
            string[] result = KnownIdentityProviders.Last(100);

            Assert.Equal(6, result.Length);
        }

        [Fact]
        public void All_ContainsNoDuplicates()
        {
            string[] providers = KnownIdentityProviders.All;
            HashSet<string> uniqueProviders = new HashSet<string>(providers);

            Assert.Equal(providers.Length, uniqueProviders.Count);
        }

        [Fact]
        public void All_ContainsNoNullOrEmptyValues()
        {
            string[] providers = KnownIdentityProviders.All;

            Assert.All(providers, provider => Assert.False(string.IsNullOrWhiteSpace(provider)));
        }
    }
}
