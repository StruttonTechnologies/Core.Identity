using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;
using StruttonTechnologies.Core.Identity.Domain.Models;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace StruttonTechnologies.Core.Identity.Stub.Factories
{
    /// <summary>
    /// Stub implementation of IRevocableTokenManager for unit testing.
    /// Simulates token generation, validation, and revocation behavior.
    /// </summary>
    public class StubRevocableTokenManager : IRevocableTokenManager
    {
        private readonly HashSet<string> _revokedAccessTokens = [];
        private readonly HashSet<string> _revokedRefreshTokens = [];

        /// <inheritdoc />
        public Task<string> GenerateAccessTokenAsync(Guid userId, string username, string email, IEnumerable<string> roles, CancellationToken cancellationToken)
        {
            ClaimsIdentity identity = new ClaimsIdentity("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(ClaimTypes.Email, email ?? string.Empty));

            foreach (string role in roles ?? Enumerable.Empty<string>())
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
                signingCredentials: creds);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        /// <inheritdoc />
        public Task<string> GenerateRefreshTokenAsync(Guid userId, string username, CancellationToken cancellationToken)
        {
            return Task.FromResult(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
        }

        /// <inheritdoc />
        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtTokenOptions options = StubJwtOptionsFactory.CreateDefault();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey));

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
                        ValidIssuer = options.Issuer,
                        ValidAudience = options.Audience,
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken? validatedToken);

                JwtSecurityToken? jwt = validatedToken as JwtSecurityToken;
                string? jti = jwt?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (jti != null && _revokedAccessTokens.Contains(token))
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

        /// <inheritdoc />
        public Task<DateTime?> GetExpirationAsync(string token)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return Task.FromResult<DateTime?>(jwt.ValidTo);
        }

        /// <inheritdoc />
        public Task RevokeAccessTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            _revokedAccessTokens.Add(token);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return Task.FromResult(_revokedAccessTokens.Contains(token));
        }

        /// <inheritdoc />
        public Task RevokeRefreshTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            _revokedRefreshTokens.Add(token);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return Task.FromResult(_revokedRefreshTokens.Contains(token));
        }
    }
}
