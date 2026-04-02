using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.JwtTokens.Mapping;

namespace StruttonTechnologies.Core.Identity.Orchestration.Tests.Mapping
{
    [ExcludeFromCodeCoverage]
    public class JwtSecurityTokenExtensionsTests
    {
        [Fact]
        public void ToTokenResponseDto_ReturnsValidDto_WhenPrincipalIsValid()
        {
            string userId = Guid.NewGuid().ToString();
            string token = "test-jwt-token";
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(30);

            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Email, "test@example.com")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            TokenResponseDto result = principal.ToTokenResponseDto(token, expiresAt);

            Assert.NotNull(result);
            Assert.Equal(userId, result.AccessToken);
            Assert.Equal(token, result.RefreshToken);
            Assert.Equal(expiresAt, result.ExpiresAt);
        }

        [Fact]
        public void ToTokenResponseDto_ThrowsArgumentNullException_WhenPrincipalIsNull()
        {
            string token = "test-token";
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(30);

            Assert.Throws<ArgumentNullException>(() =>
                JwtSecurityTokenExtensions.ToTokenResponseDto(null!, token, expiresAt));
        }

        [Fact]
        public void ToTokenResponseDto_ThrowsInvalidOperationException_WhenIdentityIsNotClaimsIdentity()
        {
            ClaimsPrincipal principal = new(new ClaimsIdentity());
            string token = "test-token";
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(30);

            Assert.Throws<InvalidOperationException>(() =>
                principal.ToTokenResponseDto(token, expiresAt));
        }

        [Fact]
        public void ToTokenResponseDto_ThrowsInvalidOperationException_WhenUserIdClaimIsMissing()
        {
            ClaimsIdentity identity = new(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Email, "test@example.com")
            }, "TestAuth");
            ClaimsPrincipal principal = new(identity);
            string token = "test-token";
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(30);

            Assert.Throws<InvalidOperationException>(() =>
                principal.ToTokenResponseDto(token, expiresAt));
        }
    }
}
