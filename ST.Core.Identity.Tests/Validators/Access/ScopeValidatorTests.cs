using ST.Core.Identity.Validators.Access;
using Xunit;

namespace ST.Core.Identity.Tests.Validators.Access
{
    public class ScopeValidatorTests
    {
        private readonly ScopeValidator _validator = new();

        public static IEnumerable<object[]> ValidScopes =>
            new List<object[]>
            {
                new object[] { new[] { "read:user", "write:profile" } },
                new object[] { new[] { "admin:tenant" } }
            };

        public static IEnumerable<object[]> InvalidScopes =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new[] { "unknown:scope" } },
                new object[] { new[] { "read:user", "invalid:scope" } }
            };

        [Theory]
        [MemberData(nameof(ValidScopes))]
        public void Should_Return_Success_For_Valid_Scopes(IEnumerable<string> scopes)
        {
            var result = _validator.Validate(scopes);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [MemberData(nameof(InvalidScopes))]
        public void Should_Return_Failure_For_Invalid_Or_Missing_Scopes(IEnumerable<string> scopes)
        {
            var result = _validator.Validate(scopes);
            Assert.False(result.IsSuccess);
        }
    }
}