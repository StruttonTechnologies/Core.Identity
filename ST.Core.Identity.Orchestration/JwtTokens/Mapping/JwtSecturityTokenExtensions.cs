using ST.Core.Identity.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Orchestration.JwtTokens.Mapping
{
    public static class JwtSecurityTokenExtensions
    {

        public static TokenResponseDto ToTokenResponseDto(this ClaimsPrincipal principal, string token, DateTime expiresAt)
        {
            var identity = principal.Identity as ClaimsIdentity
                ?? throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity.");

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("UserId claim is missing.");

            return new TokenResponseDto(userId, token, expiresAt);
        }
    }
}
