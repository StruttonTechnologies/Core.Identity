using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManagement
{
    public abstract partial class AuthenticationUserService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously generates a user token for a specific purpose using the given provider.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity for whom to generate the token. Must not be null.</param>
        /// <param name="tokenProvider">The token provider. Must not be null or empty.</param>
        /// <param name="purpose">The purpose for which the token is generated. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The generated user token as a string.
        /// Returns <c>null</c> if an exception occurs.
        /// </returns>
        public virtual async Task<string> GenerateUserTokenAsync(TUser user, string tokenProvider, string purpose, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(tokenProvider))
                throw new ArgumentException("Token provider must not be null or empty.", nameof(tokenProvider));
            if (string.IsNullOrEmpty(purpose))
                throw new ArgumentException("Purpose must not be null or empty.", nameof(purpose));

            try
            {
                return await _userManager.GenerateUserTokenAsync(user, tokenProvider, purpose);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate user token for user {UserId} with provider {Provider} and purpose {Purpose}", user?.Id, tokenProvider, purpose);
                return null!;
            }
        }
    }
}