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
        /// Asynchronously generates an email confirmation token for the specified user.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity for whom to generate the token. Must not be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>
        /// The generated email confirmation token as a string.
        /// Returns <c>null</c> if an exception occurs.
        /// </returns>
        public virtual async Task<string> GenerateEmailConfirmationTokenAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id.ToString()!);
                if (existingUser == null)
                    throw new InvalidOperationException($"User {user.Id} not found in store.");

                return await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate email confirmation token for user {UserId}", user.Id);
                return null!;
            }
        }
    }
}