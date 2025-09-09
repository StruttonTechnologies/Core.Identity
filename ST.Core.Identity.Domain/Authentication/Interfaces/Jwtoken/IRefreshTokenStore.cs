using ST.Core.Identity.Domain.Authorization.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.Jwtoken
{
    /// <summary>
    /// Defines methods for storing, retrieving, and revoking refresh tokens.
    /// </summary>
    public interface IRefreshTokenStore
    {
        /// <summary>
        /// Saves a refresh token asynchronously.
        /// </summary>
        /// <param name="token">The refresh token to save.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        Task SaveAsync(RefreshToken token, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a refresh token by its token string asynchronously.
        /// </summary>
        /// <param name="token">The token string to search for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="RefreshToken"/> if found; otherwise, null.</returns>
        Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken);

        /// <summary>
        /// Revokes a refresh token by its token string asynchronously.
        /// </summary>
        /// <param name="token">The token string to revoke.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        Task RevokeAsync(string token, CancellationToken cancellationToken);
    }
}
