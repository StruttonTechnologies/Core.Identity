using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Tests.Data
{
    [ExcludeFromCodeCoverage]
    public class KnownScopesTests
    {
        [Fact]
        public void OpenId_HasCorrectValue()
        {
            Assert.Equal("openid", KnownScopes.OpenId);
        }

        [Fact]
        public void Profile_HasCorrectValue()
        {
            Assert.Equal("profile", KnownScopes.Profile);
        }

        [Fact]
        public void ApiRead_HasCorrectValue()
        {
            Assert.Equal("api.read", KnownScopes.ApiRead);
        }

        [Fact]
        public void ApiWrite_HasCorrectValue()
        {
            Assert.Equal("api.write", KnownScopes.ApiWrite);
        }

        [Fact]
        public void All_ContainsAllScopes()
        {
            string[] scopes = KnownScopes.All;

            Assert.Contains(KnownScopes.OpenId, scopes);
            Assert.Contains(KnownScopes.Profile, scopes);
            Assert.Contains(KnownScopes.ApiRead, scopes);
            Assert.Contains(KnownScopes.ApiWrite, scopes);
        }

        [Fact]
        public void All_HasCorrectCount()
        {
            string[] scopes = KnownScopes.All;

            Assert.Equal(4, scopes.Length);
        }

        [Fact]
        public void All_HasCorrectOrder()
        {
            string[] scopes = KnownScopes.All;

            Assert.Equal(KnownScopes.OpenId, scopes[0]);
            Assert.Equal(KnownScopes.Profile, scopes[1]);
            Assert.Equal(KnownScopes.ApiRead, scopes[2]);
            Assert.Equal(KnownScopes.ApiWrite, scopes[3]);
        }

        [Fact]
        public void First_WithDefaultParameter_ReturnsOpenIdScope()
        {
            string[] result = KnownScopes.First();

            Assert.Single(result);
            Assert.Equal(KnownScopes.OpenId, result[0]);
        }

        [Fact]
        public void First_WithCount2_ReturnsFirstTwoScopes()
        {
            string[] result = KnownScopes.First(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownScopes.OpenId, result[0]);
            Assert.Equal(KnownScopes.Profile, result[1]);
        }

        [Fact]
        public void First_WithZeroCount_ReturnsOneScope()
        {
            string[] result = KnownScopes.First(0);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithNegativeCount_ReturnsOneScope()
        {
            string[] result = KnownScopes.First(-1);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithCountGreaterThanTotal_ReturnsAllScopes()
        {
            string[] result = KnownScopes.First(100);

            Assert.Equal(4, result.Length);
        }

        [Fact]
        public void Last_WithDefaultParameter_ReturnsApiWriteScope()
        {
            string[] result = KnownScopes.Last();

            Assert.Single(result);
            Assert.Equal(KnownScopes.ApiWrite, result[0]);
        }

        [Fact]
        public void Last_WithCount2_ReturnsLastTwoScopes()
        {
            string[] result = KnownScopes.Last(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownScopes.ApiRead, result[0]);
            Assert.Equal(KnownScopes.ApiWrite, result[1]);
        }

        [Fact]
        public void Last_WithZeroCount_ReturnsEmptyArray()
        {
            string[] result = KnownScopes.Last(0);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithNegativeCount_ReturnsEmptyArray()
        {
            string[] result = KnownScopes.Last(-1);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithCountGreaterThanTotal_ReturnsAllScopes()
        {
            string[] result = KnownScopes.Last(100);

            Assert.Equal(4, result.Length);
        }

        [Fact]
        public void All_ContainsNoDuplicates()
        {
            string[] scopes = KnownScopes.All;
            HashSet<string> uniqueScopes = new HashSet<string>(scopes);

            Assert.Equal(scopes.Length, uniqueScopes.Count);
        }

        [Fact]
        public void All_ContainsNoNullOrEmptyValues()
        {
            string[] scopes = KnownScopes.All;

            Assert.All(scopes, scope => Assert.False(string.IsNullOrWhiteSpace(scope)));
        }
    }
}
