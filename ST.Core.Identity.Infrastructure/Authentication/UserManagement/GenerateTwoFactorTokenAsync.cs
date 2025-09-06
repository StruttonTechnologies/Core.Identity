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
        /// Asynchronously generates a two-factor authentication token for the specified user using the given provider.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity for whom to generate the token. Must not be null.</param>
        /// <param name="tokenProvider">The two-factor token provider. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The generated two-factor authentication token as a string.
        /// Returns <c>null</c> if an exception occurs.
        /// </returns>
        public virtual async Task<string> GenerateTwoFactorTokenAsync(TUser user, string tokenProvider, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(tokenProvider))
                throw new ArgumentException("Token provider must not be null or empty.", nameof(tokenProvider));

            try
            {
                return await _userManager.GenerateTwoFactorTokenAsync(user, tokenProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate two-factor token for user {UserId} with provider {Provider}", user?.Id, tokenProvider);
                return null!;
            }
        }
    }
}