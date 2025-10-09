using ST.Core.Identity.Fakes.Builders;
using System;
using System.Security.Claims;
using Xunit;

namespace ST.Core.Identity.Fakes.Tests.Builders
{
    public class TestClaimsPrincipalBuilderTests
    {
        [Fact]
        public void CreateDefault_ReturnsClaimsPrincipalWithExpectedClaims()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();

            Assert.NotNull(principal);
            var identity = Assert.IsType<ClaimsIdentity>(principal.Identity);
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
            var principal = TestClaimsPrincipalBuilder.CreateDefault();
            var identity = (ClaimsIdentity)principal.Identity!;

            Assert.Contains(identity.Claims, c => c.Type == claimType);
        }

        [Fact]
        public void CreateDefault_NameIdentifier_IsGuid()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();
            var identity = (ClaimsIdentity)principal.Identity!;
            var nameIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            Assert.NotNull(nameIdClaim);
            Assert.True(Guid.TryParse(nameIdClaim!.Value, out _));
        }
    }
}