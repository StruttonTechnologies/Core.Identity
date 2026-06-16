using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.JwtTokens.Mapping;

namespace StruttonTechnologies.Core.Identity.Tests.Orchestration.Mapping;

[ExcludeFromCodeCoverage]
public class JwtSecurityTokenExtensionsTests
{
    [Fact]
    public void ToTokenResponseDto_ReturnsValidDto_WhenPrincipalIsValid()
    {
        string userId = Guid.NewGuid().ToString();
        string accessToken = "test-access-token";
        string refreshToken = "test-refresh-token";
        DateTime accessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(30);
        DateTime refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);

        ClaimsIdentity identity = new(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Email, "test@example.com"),
        }, "TestAuth");

        ClaimsPrincipal principal = new(identity);

        TokenResponseDto result = principal.ToTokenResponseDto(
            accessToken,
            refreshToken,
            accessTokenExpiresAtUtc,
            refreshTokenExpiresAtUtc);

        Assert.NotNull(result);
        Assert.Equal(accessToken, result.AccessToken);
        Assert.Equal(refreshToken, result.RefreshToken);
        Assert.Equal(accessTokenExpiresAtUtc, result.AccessTokenExpiresAtUtc);
        Assert.Equal(refreshTokenExpiresAtUtc, result.RefreshTokenExpiresAtUtc);
    }

    [Fact]
    public void ToTokenResponseDto_ThrowsArgumentNullException_WhenPrincipalIsNull()
    {
        string accessToken = "test-access-token";
        string refreshToken = "test-refresh-token";
        DateTime accessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(30);
        DateTime refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);

        Assert.Throws<ArgumentNullException>(() =>
            JwtSecurityTokenExtensions.ToTokenResponseDto(
                null!,
                accessToken,
                refreshToken,
                accessTokenExpiresAtUtc,
                refreshTokenExpiresAtUtc));
    }

    [Fact]
    public void ToTokenResponseDto_ThrowsInvalidOperationException_WhenIdentityIsNotClaimsIdentity()
    {
        ClaimsPrincipal principal = new(new ClaimsIdentity());
        string accessToken = "test-access-token";
        string refreshToken = "test-refresh-token";
        DateTime accessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(30);
        DateTime refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);

        Assert.Throws<InvalidOperationException>(() =>
            principal.ToTokenResponseDto(
                accessToken,
                refreshToken,
                accessTokenExpiresAtUtc,
                refreshTokenExpiresAtUtc));
    }

    [Fact]
    public void ToTokenResponseDto_ThrowsInvalidOperationException_WhenUserIdClaimIsMissing()
    {
        ClaimsIdentity identity = new(new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Email, "test@example.com"),
        }, "TestAuth");

        ClaimsPrincipal principal = new(identity);
        string accessToken = "test-access-token";
        string refreshToken = "test-refresh-token";
        DateTime accessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(30);
        DateTime refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);

        Assert.Throws<InvalidOperationException>(() =>
            principal.ToTokenResponseDto(
                accessToken,
                refreshToken,
                accessTokenExpiresAtUtc,
                refreshTokenExpiresAtUtc));
    }
}
