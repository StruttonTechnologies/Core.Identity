using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Tests.Data
{
    [ExcludeFromCodeCoverage]
    public class KnownReservedUsernamesTests
    {
        [Fact]
        public void All_ReturnsNonEmptyArray()
        {
            string[] usernames = KnownReservedUsernames.All;

            Assert.NotNull(usernames);
            Assert.NotEmpty(usernames);
        }

        [Fact]
        public void All_ContainsReservedUsernames()
        {
            string[] usernames = KnownReservedUsernames.All;

            Assert.Contains("admin", usernames);
            Assert.Contains("administrator", usernames);
            Assert.Contains("system", usernames);
            Assert.Contains("root", usernames);
            Assert.Contains("service", usernames);
            Assert.Contains("support", usernames);
            Assert.Contains("moderator", usernames);
        }

        [Fact]
        public void All_HasCorrectCount()
        {
            string[] usernames = KnownReservedUsernames.All;

            Assert.Equal(7, usernames.Length);
        }

        [Fact]
        public void First_WithDefaultParameter_ReturnsOneUsername()
        {
            string[] result = KnownReservedUsernames.First();

            Assert.Single(result);
            Assert.Equal(KnownReservedUsernames.All[0], result[0]);
        }

        [Fact]
        public void First_WithSpecificCount_ReturnsCorrectNumber()
        {
            string[] result = KnownReservedUsernames.First(3);

            Assert.Equal(3, result.Length);
            Assert.Equal(KnownReservedUsernames.All[0], result[0]);
            Assert.Equal(KnownReservedUsernames.All[1], result[1]);
            Assert.Equal(KnownReservedUsernames.All[2], result[2]);
        }

        [Fact]
        public void First_WithZeroCount_ReturnsOneUsername()
        {
            string[] result = KnownReservedUsernames.First(0);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithNegativeCount_ReturnsOneUsername()
        {
            string[] result = KnownReservedUsernames.First(-5);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithCountGreaterThanTotal_ReturnsAllUsernames()
        {
            string[] result = KnownReservedUsernames.First(100);

            Assert.Equal(KnownReservedUsernames.All.Length, result.Length);
        }

        [Fact]
        public void Last_WithDefaultParameter_ReturnsOneUsername()
        {
            string[] result = KnownReservedUsernames.Last();

            Assert.Single(result);
            Assert.Equal(KnownReservedUsernames.All[^1], result[0]);
        }

        [Fact]
        public void Last_WithSpecificCount_ReturnsCorrectNumber()
        {
            string[] result = KnownReservedUsernames.Last(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownReservedUsernames.All[^2], result[0]);
            Assert.Equal(KnownReservedUsernames.All[^1], result[1]);
        }

        [Fact]
        public void Last_WithZeroCount_ReturnsEmptyArray()
        {
            string[] result = KnownReservedUsernames.Last(0);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithNegativeCount_ReturnsEmptyArray()
        {
            string[] result = KnownReservedUsernames.Last(-5);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithCountGreaterThanTotal_ReturnsAllUsernames()
        {
            string[] result = KnownReservedUsernames.Last(100);

            Assert.Equal(KnownReservedUsernames.All.Length, result.Length);
        }

        [Fact]
        public void All_ContainsNoDuplicates()
        {
            string[] usernames = KnownReservedUsernames.All;
            HashSet<string> uniqueUsernames = new HashSet<string>(usernames, StringComparer.OrdinalIgnoreCase);

            Assert.Equal(usernames.Length, uniqueUsernames.Count);
        }

        [Fact]
        public void All_ContainsNoNullOrEmptyValues()
        {
            string[] usernames = KnownReservedUsernames.All;

            Assert.All(usernames, username => Assert.False(string.IsNullOrWhiteSpace(username)));
        }

        [Fact]
        public void All_UsesLowercaseValues()
        {
            string[] usernames = KnownReservedUsernames.All;

            Assert.All(usernames, username => Assert.Equal(username.ToLowerInvariant(), username));
        }
    }
}
