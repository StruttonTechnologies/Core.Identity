using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ST.Core.Identity.Application.Authentication.Mapping
{
    /// <summary>
    /// Extension methods for <see cref="ClaimsPrincipal"/> to map authentication data.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Maps a <see cref="ClaimsPrincipal"/> to a <see cref="LoginResponseDto"/>.
        /// </summary>
        public static LoginResponseDto ToDto(this ClaimsPrincipal principal, string token, DateTime expiresAt)
        {
            var identity = principal.Identity as ClaimsIdentity;

            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = identity?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            var roles = identity?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? Array.Empty<string>();

            return new LoginResponseDto(
                identity?.IsAuthenticated ?? false,
                token,
                expiresAt,
                roles,
                Guid.TryParse(userId, out var parsedId) ? parsedId : Guid.Empty,
                username
            );


        }

        /// <summary>
        /// Generates a JWT token from a <see cref="ClaimsPrincipal"/> using the provided <see cref="JwtTokenOptions"/>.
        /// </summary>
        public static string ToToken(this ClaimsPrincipal principal, JwtTokenOptions options)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null)
                throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity.");

            if (!identity.Claims.Any())
                throw new InvalidOperationException("ClaimsPrincipal must contain claims to generate a token.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var creds = options.Credentials ?? new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}