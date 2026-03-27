using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Fakes.Builders;

namespace StruttonTechnologies.Core.Identity.Fakes.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class TestClaimsPrincipalBuilderTests
    {
        [Fact]
        public void CreateDefault_ReturnsClaimsPrincipalWithExpectedClaims()
        {
            ClaimsPrincipal principal = TestClaimsPrincipalBuilder.CreateDefault();

            Assert.NotNull(principal);
            ClaimsIdentity identity = Assert.IsType<ClaimsIdentity>(principal.Identity);
            Assert.Equal("TestAuthType", identity.AuthenticationType);

            Assert.True(identity.HasClaim(c => c.Type == ClaimTypes.NameIdentifier));
            Assert.True(identity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == "StubUser"));
            Assert.True(identity.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier)]
        [InlineData(ClaimTypes.Name)]
        [InlineData(ClaimTypes.Role)]
        public void CreateDefault_ContainsExpectedClaimTypes(string claimType)
        {
            ClaimsPrincipal principal = TestClaimsPrincipalBuilder.CreateDefault();
            ClaimsIdentity identity = (ClaimsIdentity)principal.Identity!;

            Assert.Contains(identity.Claims, c => c.Type == claimType);
        }

        [Fact]
        public void CreateDefault_NameIdentifier_IsGuid()
        {
            ClaimsPrincipal principal = TestClaimsPrincipalBuilder.CreateDefault();
            ClaimsIdentity identity = (ClaimsIdentity)principal.Identity!;
            Claim? nameIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            Assert.NotNull(nameIdClaim);
            Assert.True(Guid.TryParse(nameIdClaim!.Value, out _));
        }
    }
}
