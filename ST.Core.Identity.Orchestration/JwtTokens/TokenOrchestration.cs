using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Domain.Models;
using ST.Core.Identity.Orchestration.Contracts.JwtToken;
using System.Security.Claims;

namespace ST.Core.Identity.Application.Services.JwtTokens
{
    /// <summary>
    /// Service responsible for orchestrating JWT token generation and validation.
    /// </summary>
    public class TokenOrchestration<TKey> : ITokenOrchestration<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly JwtTokenOptions _options;
        private readonly ILogger<TokenOrchestration<TKey>> _logger;
        private readonly IJwtUserTokenManager<TKey> _jwtManager;

        public TokenOrchestration(
            IOptions<JwtTokenOptions> options,
            ILogger<TokenOrchestration<TKey>> logger,
            IJwtUserTokenManager<TKey> jwtManager)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
        }

        public async Task<string> GenerateTokenAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var identity = principal?.Identity as ClaimsIdentity
                ?? throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity.");

            var userIdRaw = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("Missing user ID");

            var userId = ConvertToKey(userIdRaw);
            var userName = identity.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            var email = identity.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            var roles = identity.FindAll(ClaimTypes.Role).Select(r => r.Value);

            return await _jwtManager.GenerateAccessTokenAsync(userId, userName, email, roles, cancellationToken);
        }

        public DateTime GetExpirationTime() => DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
            => _jwtManager.RevokeAccessTokenAsync(token, cancellationToken);

        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
            => _jwtManager.IsAccessTokenRevokedAsync(token, cancellationToken);

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
            => _jwtManager.RevokeRefreshTokenAsync(token, cancellationToken);

        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
            => _jwtManager.IsRefreshTokenRevokedAsync(token, cancellationToken);

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token, CancellationToken cancellationToken)
            => _jwtManager.ValidateTokenAsync(token);

        public Task<DateTime?> GetExpirationAsync(string token, CancellationToken cancellationToken)
            => _jwtManager.GetExpirationAsync(token);

        private TKey ConvertToKey(string raw)
        {
            if (typeof(TKey) == typeof(Guid))
            {
                if (Guid.TryParse(raw, out var guid))
                    return (TKey)(object)guid;

                throw new InvalidCastException($"Cannot convert '{raw}' to Guid.");
            }

            if (typeof(TKey) == typeof(int))
            {
                if (int.TryParse(raw, out var intVal))
                    return (TKey)(object)intVal;

                throw new InvalidCastException($"Cannot convert '{raw}' to int.");
            }

            if (typeof(TKey) == typeof(long))
            {
                if (long.TryParse(raw, out var longVal))
                    return (TKey)(object)longVal;

                throw new InvalidCastException($"Cannot convert '{raw}' to long.");
            }

            return (TKey)Convert.ChangeType(raw, typeof(TKey));
        }
    }
}