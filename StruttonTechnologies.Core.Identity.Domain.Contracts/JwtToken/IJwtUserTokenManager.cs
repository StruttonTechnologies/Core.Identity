using System.Security.Claims;

namespace StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken
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
        public Task<string> GenerateAccessTokenAsync(
            TKey userId,
            string username,
            string email,
            IEnumerable<string> roles,
            CancellationToken cancellationToken);

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token);

        public Task<DateTime?> GetExpirationAsync(string token);

        public Task RevokeAccessTokensAsync(TKey userId, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes a specific access token by its JTI.
        /// </summary>
        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether the specified access token has been revoked.
        /// </summary>
        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken);

        public Task<string> GenerateRefreshTokenAsync(
            TKey userId,
            string username,
            CancellationToken cancellationToken);

        public Task RevokeRefreshTokensAsync(TKey userId, CancellationToken cancellationToken);

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks whether the specified refresh token has been revoked.
        /// </summary>
        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken);
    }
}
