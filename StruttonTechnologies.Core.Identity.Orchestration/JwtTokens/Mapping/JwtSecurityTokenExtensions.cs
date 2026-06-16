using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Orchestration.JwtTokens.Mapping;

public static class JwtSecurityTokenExtensions
{
    public static TokenResponseDto ToTokenResponseDto(
        this ClaimsPrincipal principal,
        string accessToken,
        string refreshToken,
        DateTime accessTokenExpiresAtUtc,
        DateTime refreshTokenExpiresAtUtc)
    {
        ArgumentNullException.ThrowIfNull(principal);
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        return new TokenResponseDto(
            accessToken,
            refreshToken,
            accessTokenExpiresAtUtc,
            refreshTokenExpiresAtUtc);
    }
}
