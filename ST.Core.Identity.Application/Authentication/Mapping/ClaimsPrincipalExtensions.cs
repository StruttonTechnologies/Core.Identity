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
        public static LoginResponseDto ToDto(this ClaimsPrincipal principal, string accessToken, string refreshToken, DateTime expiresAt)
        {
            if (principal?.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
            {
                return new LoginResponseDto(
                    AccessToken: accessToken,
                    RefreshToken: refreshToken,
                    ExpiresAt: expiresAt,
                    Username: string.Empty,
                    Provider: "Unknown",
                    IsNewUser: false,
                    RequiresTwoFactor: false
                );
            }

            
            var username = identity.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            var provider = identity.FindFirst("provider")?.Value ?? "Local";
            var isNewUser = identity.FindFirst("is_new_user")?.Value == "true";
            var requiresTwoFactor = identity.FindFirst("requires_2fa")?.Value == "true";

            return new LoginResponseDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresAt: expiresAt,
                Username: username,
                Provider: provider,
                IsNewUser: isNewUser,
                RequiresTwoFactor: requiresTwoFactor
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