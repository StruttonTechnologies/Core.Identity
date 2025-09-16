using ST.Core.Identity.Testing.Setup.Builders;
using System.Security.Claims;
using Xunit;

namespace ST.Core.Identity.Testing.Tests.Setup.Builders
{
    /// <summary>
    /// Unit tests for <see cref="TestClaimsPrincipalBuilder"/>.
    /// Validates default claim composition and identity structure.
    /// </summary>
    public class TestClaimsPrincipalBuilderTests
    {
        [Fact]
        public void CreateDefault_ReturnsValidPrincipal()
        {
            var principal = TestClaimsPrincipalBuilder.CreateDefault();

            Assert.NotNull(principal);
            Assert.IsType<ClaimsPrincipal>(principal);
            Assert.NotNull(principal.Identity);
            Assert.True(principal.Identity.IsAuthenticated);

            var claims = principal.Claims;
            Assert.Contains(claims, c => c.Type == ClaimTypes.NameIdentifier);
            Assert.Contains(claims, c => c.Type == ClaimTypes.Name && c.Value == "testuser");
            Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }
    }
}