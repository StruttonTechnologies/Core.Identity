using Microsoft.IdentityModel.Tokens;
using ST.Core.Identity.Domain.Authentication.Entities;
using ST.Core.Identity.Domain.Authentication.Interfaces.Jwtoken;
using ST.Core.Identity.Domain.Authentication.Interfaces.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ST.Core.IdentityAccess.JwtToken
{
    [AutoRegister]
    public class JwtUserTokenManager<TKey> : IJwtUserTokenManager<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRefreshTokenStore _refreshTokenStore;

        // In-memory denylist for access tokens (JTI-based)
        private static readonly Dictionary<string, DateTime> _revokedAccessTokens = new();

        // In-memory denylist for refresh tokens
        private static readonly HashSet<string> _revokedRefreshTokens = new();

        public JwtUserTokenManager(IRefreshTokenStore refreshTokenStore)
        {
            _refreshTokenStore = refreshTokenStore;
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")); // Replace with config
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }

        public async Task<string> GenerateRefreshTokenAsync(TKey userId, string username, CancellationToken cancellationToken)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId?.ToString(),
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")); // Replace with config

            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "your-issuer",
                    ValidAudience = "your-audience",
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
            // Optional: implement user-based access token revocation if you store JTI per user
            return Task.CompletedTask;
        }

        public Task RevokeRefreshTokensAsync(TKey userId, CancellationToken cancellationToken)
        {
            // Optional: implement user-based refresh token revocation if you store tokens per user
            return Task.CompletedTask;
        }
    }
}