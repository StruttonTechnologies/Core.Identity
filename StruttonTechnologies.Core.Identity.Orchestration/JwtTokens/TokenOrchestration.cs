using System.Security.Claims;

using Microsoft.Extensions.Options;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Orchestration.JwtTokens
{
    /// <summary>
    /// Service responsible for orchestrating JWT token generation and validation.
    /// </summary>
    /// <typeparam name="TKey">The type of the user identifier used for token operations. Must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public class TokenOrchestration<TKey> : ITokenOrchestration<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly JwtTokenOptions _options;
        private readonly IJwtUserTokenManager<TKey> _jwtManager;

        public TokenOrchestration(
            IOptions<JwtTokenOptions> options,
            IJwtUserTokenManager<TKey> jwtManager)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
        }

        public async Task<string> GenerateTokenAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            ClaimsIdentity identity = principal?.Identity as ClaimsIdentity
                ?? throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity.");

            string userIdRaw = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("Missing user ID");

            TKey userId = ConvertToKey(userIdRaw);
            string userName = identity.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            string email = identity.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            IEnumerable<string> roles = identity.FindAll(ClaimTypes.Role).Select(r => r.Value);

            return await _jwtManager.GenerateAccessTokenAsync(userId, userName, email, roles, cancellationToken);
        }

        public DateTime GetExpirationTime()
        {
            return DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);
        }

        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            return _jwtManager.RevokeAccessTokenAsync(token, cancellationToken);
        }

        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return _jwtManager.IsAccessTokenRevokedAsync(token, cancellationToken);
        }

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            return _jwtManager.RevokeRefreshTokenAsync(token, cancellationToken);
        }

        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return _jwtManager.IsRefreshTokenRevokedAsync(token, cancellationToken);
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token, CancellationToken cancellationToken)
        {
            return _jwtManager.ValidateTokenAsync(token);
        }

        public Task<DateTime?> GetExpirationAsync(string token, CancellationToken cancellationToken)
        {
            return _jwtManager.GetExpirationAsync(token);
        }

        private static TKey ConvertToKey(string raw)
        {
            if (typeof(TKey) == typeof(Guid))
            {
                if (Guid.TryParse(raw, out Guid guid))
                {
                    return (TKey)(object)guid;
                }

                throw new InvalidCastException($"Cannot convert '{raw}' to Guid.");
            }

            if (typeof(TKey) == typeof(int))
            {
                if (int.TryParse(raw, out int intVal))
                {
                    return (TKey)(object)intVal;
                }

                throw new InvalidCastException($"Cannot convert '{raw}' to int.");
            }

            if (typeof(TKey) == typeof(long))
            {
                if (long.TryParse(raw, out long longVal))
                {
                    return (TKey)(object)longVal;
                }

                throw new InvalidCastException($"Cannot convert '{raw}' to long.");
            }

            // Use invariant culture to avoid locale-dependent behavior
            return (TKey)Convert.ChangeType(raw, typeof(TKey), System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
