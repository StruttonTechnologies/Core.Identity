using System;
using System.Security.Claims;

namespace ST.Core.Identity.Domain.Interfaces.Jwtoken
{
    /// <summary>
    /// Defines operations for generating and validating JWT tokens.
    /// </summary>
    public interface IJwtUserTokenManager<TKey>
    where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Generates a JWT access token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="roles">A collection of roles assigned to the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated access token as a string.</returns>
        Task<string> GenerateAccessTokenAsync(TKey userId, string username, string email, IEnumerable<string> roles, CancellationToken cancellationToken);

        /// <summary>
        /// Generates a JWT refresh token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated refresh token as a string.</returns>
        Task<string> GenerateRefreshTokenAsync(TKey userId, string username, CancellationToken cancellationToken);

        /// <summary>
        /// Validates the specified JWT token and returns the associated claims principal if valid.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the claims principal if the token is valid; otherwise, <c>null</c>.</returns>
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token);

        /// <summary>
        /// Gets the expiration date and time of the specified JWT token.
        /// </summary>
        /// <param name="token">The JWT token to inspect.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the expiration date and time if available; otherwise, <c>null</c>.</returns>
        Task<DateTime?> GetExpirationAsync(string token);

        /// <summary>
        /// Revokes all refresh tokens associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose refresh tokens should be revoked.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RevokeRefreshTokensAsync(TKey userId, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes the specified refresh token.
        /// </summary>
        /// <param name="token">The refresh token to revoke.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);


        /// <summary>
        /// Revokes all access tokens associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose access tokens should be revoked.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RevokeAccessTokensAsync(TKey userId, CancellationToken cancellationToken);
    }
}