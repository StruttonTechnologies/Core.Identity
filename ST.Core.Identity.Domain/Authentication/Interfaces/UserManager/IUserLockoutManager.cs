using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.UserManager
{
    /// <summary>
    /// Provides lockout management operations for a user.
    /// </summary>
    /// <typeparam name="TUser">The type of the user, derived from <see cref="IdentityUser"/>.</typeparam>
    public interface IUserLockoutManager<TUser>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Increments the access failed count for the specified user.
        /// </summary>
        /// <param name="user">The user whose access failed count to increment.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task<IdentityResult> AccessFailedAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of failed access attempts for the specified user.
        /// </summary>
        /// <param name="user">The user whose failed access count to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The number of failed access attempts.</returns>
        Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Resets the access failed count for the specified user.
        /// </summary>
        /// <param name="user">The user whose access failed count to reset.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task<IdentityResult> ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Determines whether the specified user is currently locked out.
        /// </summary>
        /// <param name="user">The user to check for lockout status.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>True if the user is locked out; otherwise, false.</returns>
        Task<bool> IsLockedOutAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the lockout end date and time for the specified user.
        /// </summary>
        /// <param name="user">The user whose lockout end date to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The lockout end date and time, or null if not locked out.</returns>
        Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a value indicating whether lockout is enabled for the specified user.
        /// </summary>
        /// <param name="user">The user to check for lockout enablement.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>True if lockout is enabled; otherwise, false.</returns>
        Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Sets a value indicating whether lockout is enabled for the specified user.
        /// </summary>
        /// <param name="user">The user to set lockout enablement for.</param>
        /// <param name="enabled">True to enable lockout; false to disable.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task<IdentityResult> SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken);

    }
}
