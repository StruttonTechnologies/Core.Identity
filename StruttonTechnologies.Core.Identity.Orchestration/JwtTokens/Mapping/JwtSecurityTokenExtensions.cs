using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Orchestration.JwtTokens.Mapping
{
    public static class JwtSecurityTokenExtensions
    {
        public static TokenResponseDto ToTokenResponseDto(this ClaimsPrincipal principal, string token, DateTime expiresAt)
        {
            ArgumentNullException.ThrowIfNull(principal);

            ClaimsIdentity identity = principal.Identity as ClaimsIdentity
                ?? throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity.");

            string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("UserId claim is missing.");

            return new TokenResponseDto(userId, token, expiresAt);
        }
    }
}
