using System.Globalization;
using System.Security.Claims;

using Microsoft.Extensions.Options;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Orchestration.JwtTokens;

/// <summary>
/// Service responsible for orchestrating JWT access-token and refresh-token operations.
/// </summary>
/// <typeparam name="TKey">The type of the user identifier used for token operations.</typeparam>
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
        ArgumentNullException.ThrowIfNull(principal);

        ClaimsIdentity identity = principal.Identity as ClaimsIdentity
            ?? throw new InvalidOperationException("ClaimsPrincipal must have a ClaimsIdentity.");

        string userIdRaw = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("Missing user ID.");

        TKey userId = ConvertToKey(userIdRaw);
        string userName = identity.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        string email = identity.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        IEnumerable<string> roles = identity.FindAll(ClaimTypes.Role).Select(r => r.Value);

        return await _jwtManager.GenerateAccessTokenAsync(userId, userName, email, roles, cancellationToken);
    }

    public DateTime GetExpirationTime()
    {
        return DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);
    }

    public Task<string> GenerateRefreshTokenAsync(
        TKey userId,
        string username,
        CancellationToken cancellationToken = default)
    {
        return _jwtManager.GenerateRefreshTokenAsync(userId, username, cancellationToken);
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
            return Guid.TryParse(raw, out Guid guid)
                ? (TKey)(object)guid
                : throw new InvalidCastException($"Cannot convert '{raw}' to Guid.");
        }

        if (typeof(TKey) == typeof(int))
        {
            return int.TryParse(raw, out int intVal)
                ? (TKey)(object)intVal
                : throw new InvalidCastException($"Cannot convert '{raw}' to int.");
        }

        if (typeof(TKey) == typeof(long))
        {
            return long.TryParse(raw, out long longVal)
                ? (TKey)(object)longVal
                : throw new InvalidCastException($"Cannot convert '{raw}' to long.");
        }

        return (TKey)Convert.ChangeType(raw, typeof(TKey), CultureInfo.InvariantCulture);
    }
}
