using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Application.Services.LockOut
{
    /// <summary>
    /// Provides lockout-related operations for user accounts.
    /// </summary>
    /// <typeparam name="TUser">The user entity type.</typeparam>
    public class LockoutService<TUser> : ILockoutService where TUser : IdentityUser
    {
        private readonly UserManager<TUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockoutService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager instance.</param>
        public LockoutService(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Increments the access failed count for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found.</exception>
        public async Task AccessFailedAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new InvalidOperationException($"User not found with ID '{userId}'.");

            await _userManager.AccessFailedAsync(user);
        }

        /// <summary>
        /// Gets the access failed count for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The number of failed access attempts.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found.</exception>
        public async Task<int> GetAccessFailedCountAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new InvalidOperationException($"User not found with ID '{userId}'.");

            return user.AccessFailedCount;
        }

        /// <summary>
        /// Gets the lockout end date for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The lockout end date, or null if not locked out.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found.</exception>
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new InvalidOperationException($"User not found with ID '{userId}'.");

            return user.LockoutEnd;
        }

        /// <summary>
        /// Resets the access failed count for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found.</exception>
        public async Task ResetAccessFailedCountAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new InvalidOperationException($"User not found with ID '{userId}'.");

            await _userManager.ResetAccessFailedCountAsync(user);
        }

        /// <summary>
        /// Sets whether lockout is enabled for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="enabled">True to enable lockout; otherwise, false.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found.</exception>
        public async Task SetLockoutEnabledAsync(Guid userId, bool enabled, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new InvalidOperationException($"User not found with ID '{userId}'.");

            await _userManager.SetLockoutEnabledAsync(user, enabled);
        }

        /// <summary>
        /// Sets the lockout end date for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="lockoutEnd">The lockout end date.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found.</exception>
        public async Task SetLockoutEndDateAsync(Guid userId, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new InvalidOperationException($"User not found with ID '{userId}'.");

            await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        }

        /// <summary>
        /// Determines whether the specified user is locked out.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>True if the user is locked out; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found.</exception>
        public async Task<bool> IsLockedOutAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new InvalidOperationException($"User not found with ID '{userId}'.");

            return await _userManager.IsLockedOutAsync(user);
        }
    }
}
