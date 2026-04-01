using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Tests.Data
{
    [ExcludeFromCodeCoverage]
    public class KnownClaimsTests
    {
        [Fact]
        public void Email_HasCorrectValue()
        {
            Assert.Equal("email", KnownClaims.Email);
        }

        [Fact]
        public void Role_HasCorrectValue()
        {
            Assert.Equal("role", KnownClaims.Role);
        }

        [Fact]
        public void All_ContainsAllClaims()
        {
            string[] claims = KnownClaims.All;

            Assert.Contains(KnownClaims.Email, claims);
            Assert.Contains(KnownClaims.Role, claims);
        }

        [Fact]
        public void All_HasCorrectCount()
        {
            string[] claims = KnownClaims.All;

            Assert.Equal(2, claims.Length);
        }

        [Fact]
        public void All_HasCorrectOrder()
        {
            string[] claims = KnownClaims.All;

            Assert.Equal(KnownClaims.Email, claims[0]);
            Assert.Equal(KnownClaims.Role, claims[1]);
        }

        [Fact]
        public void First_WithDefaultParameter_ReturnsEmailClaim()
        {
            string[] result = KnownClaims.First();

            Assert.Single(result);
            Assert.Equal(KnownClaims.Email, result[0]);
        }

        [Fact]
        public void First_WithCount2_ReturnsBothClaims()
        {
            string[] result = KnownClaims.First(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownClaims.Email, result[0]);
            Assert.Equal(KnownClaims.Role, result[1]);
        }

        [Fact]
        public void First_WithZeroCount_ReturnsOneClaim()
        {
            string[] result = KnownClaims.First(0);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithNegativeCount_ReturnsOneClaim()
        {
            string[] result = KnownClaims.First(-1);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithCountGreaterThanTotal_ReturnsAllClaims()
        {
            string[] result = KnownClaims.First(10);

            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void Last_WithDefaultParameter_ReturnsRoleClaim()
        {
            string[] result = KnownClaims.Last();

            Assert.Single(result);
            Assert.Equal(KnownClaims.Role, result[0]);
        }

        [Fact]
        public void Last_WithCount2_ReturnsBothClaims()
        {
            string[] result = KnownClaims.Last(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownClaims.Email, result[0]);
            Assert.Equal(KnownClaims.Role, result[1]);
        }

        [Fact]
        public void Last_WithZeroCount_ReturnsEmptyArray()
        {
            string[] result = KnownClaims.Last(0);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithNegativeCount_ReturnsEmptyArray()
        {
            string[] result = KnownClaims.Last(-1);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithCountGreaterThanTotal_ReturnsAllClaims()
        {
            string[] result = KnownClaims.Last(10);

            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void All_ContainsNoDuplicates()
        {
            string[] claims = KnownClaims.All;
            HashSet<string> uniqueClaims = new HashSet<string>(claims);

            Assert.Equal(claims.Length, uniqueClaims.Count);
        }

        [Fact]
        public void All_ContainsNoNullOrEmptyValues()
        {
            string[] claims = KnownClaims.All;

            Assert.All(claims, claim => Assert.False(string.IsNullOrWhiteSpace(claim)));
        }
    }
}
