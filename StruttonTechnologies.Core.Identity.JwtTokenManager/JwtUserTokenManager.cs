using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;
using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Domain.Interfaces.Jwtoken;
using StruttonTechnologies.Core.Identity.Domain.Models;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace StruttonTechnologies.Core.Identity.JwtTokenManager
{
    public class JwtUserTokenManager<TKey> : IJwtUserTokenManager<TKey>
        where TKey : IEquatable<TKey>
    {
        private static readonly Dictionary<string, DateTime> _revokedAccessTokens = [];
        private static readonly HashSet<string> _revokedRefreshTokens = [];

        private readonly IRefreshTokenStore<TKey> _refreshTokenStore;
        private readonly JwtTokenOptions _options;

        public JwtUserTokenManager(IRefreshTokenStore<TKey> refreshTokenStore, JwtTokenOptions options)
        {
            _refreshTokenStore = refreshTokenStore;
            _options = options;
        }

        public Task<string> GenerateAccessTokenAsync(
            TKey userId,
            string username,
            string email,
            IEnumerable<string> roles,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(username, nameof(username));
            ClaimsIdentity identity = new ClaimsIdentity("Identity.Application");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId?.ToString() ?? string.Empty));
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(ClaimTypes.Email, email ?? string.Empty));

            foreach (string role in roles ?? Enumerable.Empty<string>())
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            string jti = Guid.NewGuid().ToString();
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, jti));

            // Build signing credentials from SigningKey
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
                signingCredentials: creds);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }

        public async Task<string> GenerateRefreshTokenAsync(TKey userId, string username, CancellationToken cancellationToken)
        {
            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            RefreshToken<TKey> refreshToken = new RefreshToken<TKey>
            {
                Token = token,
                UserId = userId,
                Username = username,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            await _refreshTokenStore.SaveAsync(refreshToken, cancellationToken);
            return token;
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));

            try
            {
                ClaimsPrincipal principal = handler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _options.Issuer,
                        ValidAudience = _options.Audience,
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken? validatedToken);

                JwtSecurityToken? jwt = validatedToken as JwtSecurityToken;
                string? jti = jwt?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (jti != null && _revokedAccessTokens.ContainsKey(jti))
                {
                    return Task.FromResult<ClaimsPrincipal?>(null);
                }

                return Task.FromResult<ClaimsPrincipal?>(principal);
            }
            catch (SecurityTokenException)
            {
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
            catch (ArgumentException)
            {
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
        }

        public Task<DateTime?> GetExpirationAsync(string token)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return Task.FromResult<DateTime?>(jwt.ValidTo);
        }

        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string? jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (!string.IsNullOrEmpty(jti))
            {
                _revokedAccessTokens[jti] = jwt.ValidTo;
            }

            return Task.CompletedTask;
        }

        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string? jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            bool isRevoked = jti != null && _revokedAccessTokens.ContainsKey(jti);
            return Task.FromResult(isRevoked);
        }

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            _revokedRefreshTokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            bool isRevoked = _revokedRefreshTokens.Contains(token);
            return Task.FromResult(isRevoked);
        }

        public Task RevokeAccessTokensAsync(TKey userId, CancellationToken cancellationToken)
        {
            // Could be extended to revoke all tokens for a user
            return Task.CompletedTask;
        }

        public Task RevokeRefreshTokensAsync(TKey userId, CancellationToken cancellationToken)
        {
            // Could be extended to revoke all refresh tokens for a user
            return Task.CompletedTask;
        }
    }
}
