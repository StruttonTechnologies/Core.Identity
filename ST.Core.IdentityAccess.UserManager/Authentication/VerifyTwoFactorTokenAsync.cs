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
        /// Asynchronously verifies a two-factor authentication token for the specified user and provider.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to verify the token for. Must not be null.</param>
        /// <param name="tokenProvider">The two-factor token provider. Must not be null or empty.</param>
        /// <param name="token">The token to verify. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// <c>true</c> if the token is valid; otherwise, <c>false</c>.
        /// Returns <c>false</c> if an exception occurs.
        /// </returns>
        public virtual async Task<bool> VerifyTwoFactorTokenAsync(TUser user, string tokenProvider, string token, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNullOrEmpty(tokenProvider);
            ArgumentNullException.ThrowIfNullOrEmpty(token);

            try
            {
                return await _userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify two-factor token for user {UserId} with provider {Provider}", user.Id, tokenProvider);
                return false;
            }
        }
    }
}