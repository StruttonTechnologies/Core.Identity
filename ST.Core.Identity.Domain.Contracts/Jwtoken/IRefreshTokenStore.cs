using ST.Core.Identity.Domain.Entities;

namespace ST.Core.Identity.Domain.Interfaces.Jwtoken
{
    /// <summary>
    /// Defines operations for storing and managing refresh tokens.
    /// </summary>
    /// <typeparam name="TKey">The type of the user's unique identifier.</typeparam>
    public interface IRefreshTokenStore<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Saves a refresh token asynchronously.
        /// </summary>
        /// <param name="token">The refresh token to save.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        Task SaveAsync(RefreshToken<TKey> token, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a refresh token by its token string asynchronously.
        /// </summary>
        /// <param name="token">The token string to search for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="RefreshToken{TKey}"/> if found; otherwise, null.</returns>
        Task<RefreshToken<TKey>?> GetAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes a refresh token by its token string asynchronously.
        /// </summary>
        /// <param name="token">The token string to revoke.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        Task RevokeAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes all refresh tokens associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        Task RevokeAllAsync(TKey userId, CancellationToken cancellationToken);
    } 
}