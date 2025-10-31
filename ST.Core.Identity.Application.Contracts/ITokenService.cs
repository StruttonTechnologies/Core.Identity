using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Contracts
{
    /// <summary>
    /// Default orchestration service for Guid-based JWT tokens.
    /// </summary>
    public interface ITokenService : ITokenService<Guid>
    {
    }

    /// <summary>
    /// Orchestration service for generating, validating, and revoking JWT tokens.
    /// </summary>
    public interface ITokenService<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Asynchronously generates a JWT access token from a ClaimsPrincipal.
        /// </summary>
        Task<string> GenerateTokenAsync(ClaimsPrincipal principal);

        /// <summary>
        /// Gets the expiration time for newly issued tokens.
        /// </summary>
        DateTime GetExpirationTime();

        /// <summary>
        /// Revokes a specific access token.
        /// </summary>
        Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether the specified access token has been revoked.
        /// </summary>
        Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes a specific refresh token.
        /// </summary>
        Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether the specified refresh token has been revoked.
        /// </summary>
        Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Validates a token and returns its ClaimsPrincipal if valid.
        /// </summary>
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Extracts the expiration timestamp from a token.
        /// </summary>
        Task<DateTime?> GetExpirationAsync(string token, CancellationToken cancellationToken);
    }
}