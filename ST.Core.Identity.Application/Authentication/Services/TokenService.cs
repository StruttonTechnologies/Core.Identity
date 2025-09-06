using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Authentication.Interfaces;
using ST.Core.Identity.Application.Authentication.Mapping;
using ST.Core.Identity.Application.Authentication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ST.Core.Identity.Application.Authentication.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenOptions _options;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService(IOptions<JwtTokenOptions> options)
        {
            _options = options.Value;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(ClaimsPrincipal principal)
        {
            return principal.ToToken(_options); 
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var key = _options.Credentials?.Key ?? new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.Key));

            try
            {
                var principal = _tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _options.Issuer,
                    ValidAudience = _options.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public DateTime? GetExpiration(string token)
        {
            var jwt = _tokenHandler.ReadJwtToken(token);
            return jwt.ValidTo;
        }
    }
}