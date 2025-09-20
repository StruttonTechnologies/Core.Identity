using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    public abstract partial class AuthenticationUserService<TUser, TKey>
         where TUser : IdentityUser<TKey>, new()
         where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Asynchronously verifies a user token for a specific purpose using the given provider.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to verify the token for. Must not be null.</param>
        /// <param name="tokenProvider">The token provider. Must not be null or empty.</param>
        /// <param name="purpose">The purpose for which the token was generated. Must not be null or empty.</param>
        /// <param name="token">The token to verify. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if the token is valid; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> VerifyUserTokenAsync(TUser user, string tokenProvider, string purpose, string token, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNullOrEmpty(tokenProvider);
            ArgumentNullException.ThrowIfNullOrEmpty(purpose);
            ArgumentNullException.ThrowIfNullOrEmpty(token);

            try
            {
                return await _userManager.VerifyUserTokenAsync(user, tokenProvider, purpose, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify user token for user {UserId} with provider {Provider} and purpose {Purpose}", user.Id, tokenProvider, purpose);
                return false;
            }
        }
    }
}