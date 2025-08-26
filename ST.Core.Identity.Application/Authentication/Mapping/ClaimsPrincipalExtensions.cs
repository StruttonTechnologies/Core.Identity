using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="principal">The claims principal to map.</param>
        /// <param name="token">The JWT token string.</param>
        /// <param name="expiresAt">The expiration date and time of the token.</param>
        /// <returns>A <see cref="LoginResponseDto"/> containing authentication details.</returns>
        public static LoginResponseDto ToDto(this ClaimsPrincipal principal, string token, DateTime expiresAt)
        {
            var identity = principal.Identity as ClaimsIdentity;

            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = identity?.FindFirst(ClaimTypes.Name)?.Value ?? "";
            var roles = identity?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? Array.Empty<string>();

            return new LoginResponseDto
            {
                IsAuthenticated = identity?.IsAuthenticated ?? false,
                Token = token,
                ExpiresAt = expiresAt,
                Roles = roles,
                UserId = Guid.TryParse(userId, out var parsedId) ? parsedId : Guid.Empty,
                Username = username
            };
        }

        /// <summary>
        /// Generates a JWT token from a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">The claims principal to generate the token from.</param>
        /// <param name="expiresAt">The expiration date and time of the token.</param>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience of the token.</param>
        /// <param name="signingKey">The signing key used to sign the token.</param>
        /// <returns>The generated JWT token string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the principal does not have a <see cref="ClaimsIdentity"/>.</exception>
        public static string ToToken(this ClaimsPrincipal principal, DateTime expiresAt, string issuer, string audience, string signingKey)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null)
                throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity");

            var claims = identity.Claims;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
