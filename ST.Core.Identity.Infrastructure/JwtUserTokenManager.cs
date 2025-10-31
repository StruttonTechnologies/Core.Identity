using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Application.Models;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Registration.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ST.Core.Identity.Infrastructure
{
    [AutoRegister]
    public class JwtUserTokenManager<TKey> : IJwtUserTokenManager<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRefreshTokenStore<TKey> _refreshTokenStore;
        private readonly JwtTokenOptions _options;

        private static readonly Dictionary<string, DateTime> _revokedAccessTokens = new();
        private static readonly HashSet<string> _revokedRefreshTokens = new();

        public JwtUserTokenManager(IRefreshTokenStore<TKey> refreshTokenStore, JwtTokenOptions options)
        {
            _refreshTokenStore = refreshTokenStore;
            _options = options;
        }

        public Task<string> GenerateAccessTokenAsync(TKey userId, string userName, string email, IEnumerable<string> roles, CancellationToken cancellationToken)
        {
            var identity = new ClaimsIdentity("Identity.Application");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId?.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim(ClaimTypes.Email, email ?? string.Empty));

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var jti = Guid.NewGuid().ToString();
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
                signingCredentials: _options.Credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }

        public async Task<string> GenerateRefreshTokenAsync(TKey userId, string username, CancellationToken cancellationToken)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken<TKey>
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
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _options.Issuer,
                    ValidAudience = _options.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwt = validatedToken as JwtSecurityToken;
                var jti = jwt?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (jti != null && _revokedAccessTokens.ContainsKey(jti))
                    return Task.FromResult<ClaimsPrincipal?>(null);

                return Task.FromResult<ClaimsPrincipal?>(principal);
            }
            catch
            {
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
        }

        public Task<DateTime?> GetExpirationAsync(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return Task.FromResult<DateTime?>(jwt.ValidTo);
        }

        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (!string.IsNullOrEmpty(jti))
            {
                _revokedAccessTokens[jti] = jwt.ValidTo;
            }

            return Task.CompletedTask;
        }

        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var isRevoked = jti != null && _revokedAccessTokens.ContainsKey(jti);
            return Task.FromResult(isRevoked);
        }

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            _revokedRefreshTokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            var isRevoked = _revokedRefreshTokens.Contains(token);
            return Task.FromResult(isRevoked);
        }

        public Task RevokeAccessTokensAsync(TKey userId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RevokeRefreshTokensAsync(TKey userId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}