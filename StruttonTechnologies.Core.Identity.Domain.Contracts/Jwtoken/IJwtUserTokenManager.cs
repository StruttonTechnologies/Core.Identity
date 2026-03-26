using System.Security.Claims;

namespace StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken
{
    public interface IRevocableTokenManager : IJwtUserTokenManager<Guid>
    {
    }

    /// <summary>
    /// Defines operations for generating, validating, and revoking JWT tokens.
    /// </summary>
    /// <typeparam name="TKey">The type of the user identifier.</typeparam>
    public interface IJwtUserTokenManager<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<string> GenerateAccessTokenAsync(
            TKey userId,
            string username,
            string email,
            IEnumerable<string> roles,
            CancellationToken cancellationToken);

        Task<ClaimsPrincipal?> ValidateTokenAsync(string token);

        Task<DateTime?> GetExpirationAsync(string token);

        Task RevokeAccessTokensAsync(TKey userId, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes a specific access token by its JTI.
        /// </summary>
        Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether the specified access token has been revoked.
        /// </summary>
        Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken);

        Task<string> GenerateRefreshTokenAsync(
            TKey userId,
            string username,
            CancellationToken cancellationToken);

        Task RevokeRefreshTokensAsync(TKey userId, CancellationToken cancellationToken);

        Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether the specified refresh token has been revoked.
        /// </summary>
        Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken);
    }
}
