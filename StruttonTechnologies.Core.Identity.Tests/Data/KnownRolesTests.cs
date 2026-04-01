using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Tests.Data
{
    [ExcludeFromCodeCoverage]
    public class KnownRolesTests
    {
        [Fact]
        public void Admin_HasCorrectValue()
        {
            Assert.Equal("Admin", KnownRoles.Admin);
        }

        [Fact]
        public void Member_HasCorrectValue()
        {
            Assert.Equal("Member", KnownRoles.Member);
        }

        [Fact]
        public void Guest_HasCorrectValue()
        {
            Assert.Equal("Guest", KnownRoles.Guest);
        }

        [Fact]
        public void All_ContainsAllRoles()
        {
            string[] roles = KnownRoles.All;

            Assert.Contains(KnownRoles.Admin, roles);
            Assert.Contains(KnownRoles.Member, roles);
            Assert.Contains(KnownRoles.Guest, roles);
        }

        [Fact]
        public void All_HasCorrectCount()
        {
            string[] roles = KnownRoles.All;

            Assert.Equal(3, roles.Length);
        }

        [Fact]
        public void All_HasCorrectOrder()
        {
            string[] roles = KnownRoles.All;

            Assert.Equal(KnownRoles.Admin, roles[0]);
            Assert.Equal(KnownRoles.Member, roles[1]);
            Assert.Equal(KnownRoles.Guest, roles[2]);
        }

        [Fact]
        public void First_WithDefaultParameter_ReturnsAdminRole()
        {
            string[] result = KnownRoles.First();

            Assert.Single(result);
            Assert.Equal(KnownRoles.Admin, result[0]);
        }

        [Fact]
        public void First_WithCount2_ReturnsFirstTwoRoles()
        {
            string[] result = KnownRoles.First(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownRoles.Admin, result[0]);
            Assert.Equal(KnownRoles.Member, result[1]);
        }

        [Fact]
        public void First_WithZeroCount_ReturnsOneRole()
        {
            string[] result = KnownRoles.First(0);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithNegativeCount_ReturnsOneRole()
        {
            string[] result = KnownRoles.First(-1);

            Assert.Single(result);
        }

        [Fact]
        public void First_WithCountGreaterThanTotal_ReturnsAllRoles()
        {
            string[] result = KnownRoles.First(10);

            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void Last_WithDefaultParameter_ReturnsGuestRole()
        {
            string[] result = KnownRoles.Last();

            Assert.Single(result);
            Assert.Equal(KnownRoles.Guest, result[0]);
        }

        [Fact]
        public void Last_WithCount2_ReturnsLastTwoRoles()
        {
            string[] result = KnownRoles.Last(2);

            Assert.Equal(2, result.Length);
            Assert.Equal(KnownRoles.Member, result[0]);
            Assert.Equal(KnownRoles.Guest, result[1]);
        }

        [Fact]
        public void Last_WithZeroCount_ReturnsEmptyArray()
        {
            string[] result = KnownRoles.Last(0);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithNegativeCount_ReturnsEmptyArray()
        {
            string[] result = KnownRoles.Last(-1);

            Assert.Empty(result);
        }

        [Fact]
        public void Last_WithCountGreaterThanTotal_ReturnsAllRoles()
        {
            string[] result = KnownRoles.Last(10);

            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void All_ContainsNoDuplicates()
        {
            string[] roles = KnownRoles.All;
            HashSet<string> uniqueRoles = new HashSet<string>(roles);

            Assert.Equal(roles.Length, uniqueRoles.Count);
        }

        [Fact]
        public void All_ContainsNoNullOrEmptyValues()
        {
            string[] roles = KnownRoles.All;

            Assert.All(roles, role => Assert.False(string.IsNullOrWhiteSpace(role)));
        }
    }
}
