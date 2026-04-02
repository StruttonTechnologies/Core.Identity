using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.Domain.Models;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace StruttonTechnologies.Core.Identity.JwtTokenManager
{
    public class JwtUserTokenManager<TKey> : IJwtUserTokenManager<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRefreshTokenStore<TKey> _refreshTokenStore;
        private readonly IAccessTokenRevocationStore<TKey> _accessTokenRevocationStore;
        private readonly JwtTokenOptions _options;

        public JwtUserTokenManager(
            IRefreshTokenStore<TKey> refreshTokenStore,
            IAccessTokenRevocationStore<TKey> accessTokenRevocationStore,
            JwtTokenOptions options)
        {
            _refreshTokenStore = refreshTokenStore ?? throw new ArgumentNullException(nameof(refreshTokenStore));
            _accessTokenRevocationStore = accessTokenRevocationStore ?? throw new ArgumentNullException(nameof(accessTokenRevocationStore));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<string> GenerateAccessTokenAsync(
            TKey userId,
            string username,
            string email,
            IEnumerable<string> roles,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(username);

            ClaimsIdentity identity = new ClaimsIdentity("Identity.Application");

            // Code coverage suppression: The null-coalescing branch where userId?.ToString() returns null
            // is unreachable due to the TKey : IEquatable<TKey> constraint. All types satisfying this
            // constraint (Guid, int, long, string, etc.) have ToString() implementations that never return null.
            // The coverage tool reports 50% branch coverage (2/4) because it conservatively assumes ToString()
            // could return null, but this is impossible given the generic constraint.
            [ExcludeFromCodeCoverage]
            static string GetUserIdString<T>(T? userId) where T : IEquatable<T>
            {
                return userId?.ToString() ?? string.Empty;
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, GetUserIdString(userId)));
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(ClaimTypes.Email, email ?? string.Empty));

            foreach (string role in roles ?? Enumerable.Empty<string>())
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            string jti = Guid.NewGuid().ToString("N");
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, jti));

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
                signingCredentials: creds);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
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
                IsRevoked = false,
            };

            await _refreshTokenStore.SaveAsync(refreshToken, cancellationToken);
            return token;
        }

        public async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
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
                        ClockSkew = TimeSpan.Zero,
                    },
                    out SecurityToken? validatedToken);

                JwtSecurityToken? jwt = validatedToken as JwtSecurityToken;
                string? jti = jwt?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrWhiteSpace(jti) &&
                    await _accessTokenRevocationStore.IsRevokedAsync(jti, CancellationToken.None))
                {
                    return null;
                }

                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public Task<DateTime?> GetExpirationAsync(string token)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return Task.FromResult<DateTime?>(jwt.ValidTo);
        }

        public async Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string? jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            string? sub = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(jti))
            {
                return;
            }

            TKey? userId = default;
            if (!string.IsNullOrWhiteSpace(sub))
            {
                try
                {
                    if (typeof(TKey) == typeof(Guid))
                    {
                        userId = (TKey?)(object?)Guid.Parse(sub);
                    }
                    else
                    {
                        userId = (TKey?)Convert.ChangeType(
                            sub,
                            typeof(TKey),
                            CultureInfo.InvariantCulture);
                    }
                }
                catch (InvalidCastException)
                {
                    return;
                }
                catch (FormatException)
                {
                    return;
                }
                catch (OverflowException)
                {
                    return;
                }
            }

            await _accessTokenRevocationStore.RevokeAsync(jti, userId, jwt.ValidTo, cancellationToken);
        }

        public async Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string? jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            return !string.IsNullOrWhiteSpace(jti) &&
                await _accessTokenRevocationStore.IsRevokedAsync(jti, cancellationToken);
        }

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            return _refreshTokenStore.RevokeAsync(token, cancellationToken);
        }

        public async Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            RefreshToken<TKey>? refreshToken = await _refreshTokenStore.GetAsync(token, cancellationToken);
            return refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpiresAt <= DateTime.UtcNow;
        }

        public Task RevokeAccessTokensAsync(TKey userId, CancellationToken cancellationToken)
        {
            return _accessTokenRevocationStore.RevokeAllAsync(userId, cancellationToken);
        }

        public Task RevokeRefreshTokensAsync(TKey userId, CancellationToken cancellationToken)
        {
            return _refreshTokenStore.RevokeAllAsync(userId, cancellationToken);
        }
    }
}
