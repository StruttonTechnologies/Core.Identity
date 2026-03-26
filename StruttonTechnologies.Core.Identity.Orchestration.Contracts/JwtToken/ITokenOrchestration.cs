using System.Security.Claims;

namespace StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken
{
    /// <summary>
    /// Contract for orchestrating JWT token generation, validation, and revocation.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to identify entities, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public interface ITokenOrchestration<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Generates a new JWT access token for the given principal.
        /// </summary>
        Task<string> GenerateTokenAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the expiration time for a newly issued token.
        /// </summary>
        DateTime GetExpirationTime();

        /// <summary>
        /// Validates a JWT token and returns the associated claims principal if valid.
        /// </summary>
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the expiration timestamp from an existing token.
        /// </summary>
        Task<DateTime?> GetExpirationAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes a specific access token.
        /// </summary>
        Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks if a given access token has been revoked.
        /// </summary>
        Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes a specific refresh token.
        /// </summary>
        Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks if a given refresh token has been revoked.
        /// </summary>
        Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken);
    }
}
