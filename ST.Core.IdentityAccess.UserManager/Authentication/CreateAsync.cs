using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Authentication
{
    /// <summary>
    /// Provides base functionality for user services.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public abstract partial class AuthenticationUserService<TUser> 
        where TUser : IdentityUser, new()
    {
        /// <summary>
        /// Asynchronously creates a new user with the specified password.
        /// Logs any errors encountered during the operation.
        /// </summary>
        /// <param name="user">The user entity to create. Must not be null.</param>
        /// <param name="password">The password for the new user. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>The created user entity.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when user creation fails due to validation or other errors.
        /// </exception>
        public virtual async Task<TUser> CreateAsync(TUser user, string password, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

            try
            {
                var result = await _userManager.CreateAsync(user);

                if (result == null)
                {
                    _logger.LogError("UserManager.CreateAsync returned null for user {UserId}", user.Id);
                    throw new InvalidOperationException("Failed to create user: result was null.");
                }



                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create user {UserId}: {Errors}", user.Id, errors);
                    throw new InvalidOperationException($"Failed to create user: {errors}");
                }


                var passwordResult = await _userManager.AddPasswordAsync(user, password);

                if (passwordResult == null)
                {
                    _logger.LogError("UserManager.AddPasswordAsync returned null for user {UserId}", user.Id);
                    throw new InvalidOperationException("Failed to set password: result was null.");
                }

                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to set password for user {UserId}: {Errors}", user.Id, errors);
                    throw new InvalidOperationException($"Failed to set password: {errors}");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating user {UserId}", user?.Id);
                throw;
            }
        }
    }
}