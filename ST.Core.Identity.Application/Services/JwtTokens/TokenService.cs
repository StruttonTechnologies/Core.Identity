using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Models;

namespace ST.Core.Identity.Application.Services.JwtTokens
{
    /// <summary>
    /// Service responsible for generating and revoking JWT access tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtTokenOptions _options;
        private readonly ILogger<TokenService> _logger;
        private static readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

        public TokenService(JwtTokenOptions options, ILogger<TokenService> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GenerateToken(ClaimsPrincipal principal)
        {
            if (principal?.Identity is not ClaimsIdentity identity)
                throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity.");

            if (!identity.Claims.Any())
                throw new InvalidOperationException("ClaimsPrincipal must contain claims to generate a token.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentials = _options.Credentials ?? new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jti = Guid.NewGuid().ToString();

            var claims = identity.Claims.ToList();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime GetExpirationTime() => DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

        public void RevokeToken(string token)
        {
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    _revokedTokens[jti] = jwt.ValidTo;
                    _logger.LogInformation("Token revoked: {Jti}", jti);
                }
                else
                {
                    _logger.LogWarning("Token revocation skipped: JTI not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to revoke token.");
            }
        }

        public bool IsTokenRevoked(string token)
        {
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var jti = jwt?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                var isRevoked = jti != null && _revokedTokens.ContainsKey(jti);
                if (isRevoked)
                {
                    _logger.LogInformation("Token is revoked: {Jti}", jti);
                }

                return isRevoked;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Token parsing failed. Treating as non-revoked.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during token revocation check.");
                return false;
            }
        }
    }
}