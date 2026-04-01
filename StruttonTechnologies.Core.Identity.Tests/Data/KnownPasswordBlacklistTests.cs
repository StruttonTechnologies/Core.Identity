using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Tests.Data
{
    [ExcludeFromCodeCoverage]
    public class KnownPasswordBlacklistTests
    {
        [Fact]
        public void All_ReturnsNonEmptyArray()
        {
            string[] passwords = KnownPasswordBlacklist.All;

            Assert.NotNull(passwords);
            Assert.NotEmpty(passwords);
        }

        [Fact]
        public void All_ContainsCommonPasswords()
        {
            string[] passwords = KnownPasswordBlacklist.All;

            Assert.Contains("password", passwords);
            Assert.Contains("123456", passwords);
            Assert.Contains("qwerty", passwords);
        }

        [Fact]
        public void First_WithDefaultParameter_ReturnsOnePassword()
        {
            string[] result = KnownPasswordBlacklist.First();

            Assert.Single(result);
            Assert.Equal(KnownPasswordBlacklist.All[0], result[0]);
        }

        [Fact]
        public void First_WithSpecificCount_ReturnsCorrectNumber()
        {
            string[] result = KnownPasswordBlacklist.First(3);

            Assert.Equal(3, result.Length);
            Assert.Equal(KnownPasswordBlacklist.All[0], result[0]);
            Assert.Equal(KnownPasswordBlacklist.All[1], result[1]);
            Assert.Equal(KnownPasswordBlacklist.All[2], result[2]);
        }

        [Fact]
        public void First_WithZeroCount_ReturnsOnePassword()
        {
            string[] result = KnownPasswordBlacklist.First(0);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithNegativeCount_ReturnsOnePassword()
        {
            string[] result = KnownPasswordBlacklist.First(-5);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithCountGreaterThanTotal_ReturnsAllPasswords()
        {
            string[] result = KnownPasswordBlacklist.First(1000);

            Assert.Equal(KnownPasswordBlacklist.All.Length, result.Length);
        }

        [Fact]
        public void Last_WithDefaultParameter_ReturnsOnePassword()
        {
            string[] result = KnownPasswordBlacklist.Last();

            Assert.Single(result);
            Assert.Equal(KnownPasswordBlacklist.All[^1], result[0]);
        }

        [Fact]
        public void Last_WithSpecificCount_ReturnsCorrectNumber()
        {
            string[] result = KnownPasswordBlacklist.Last(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownPasswordBlacklist.All[^2], result[0]);
            Assert.Equal(KnownPasswordBlacklist.All[^1], result[1]);
        }

        [Fact]
        public void Last_WithZeroCount_ReturnsEmptyArray()
        {
            string[] result = KnownPasswordBlacklist.Last(0);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithNegativeCount_ReturnsEmptyArray()
        {
            string[] result = KnownPasswordBlacklist.Last(-5);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithCountGreaterThanTotal_ReturnsAllPasswords()
        {
            string[] result = KnownPasswordBlacklist.Last(1000);

            Assert.Equal(KnownPasswordBlacklist.All.Length, result.Length);
        }

        [Fact]
        public void All_ContainsNoDuplicates()
        {
            string[] passwords = KnownPasswordBlacklist.All;
            HashSet<string> uniquePasswords = new HashSet<string>(passwords, StringComparer.OrdinalIgnoreCase);

            Assert.Equal(passwords.Length, uniquePasswords.Count);
        }

        [Fact]
        public void All_ContainsNoNullOrEmptyValues()
        {
            string[] passwords = KnownPasswordBlacklist.All;

            Assert.All(passwords, password => Assert.False(string.IsNullOrWhiteSpace(password)));
        }
    }
}
