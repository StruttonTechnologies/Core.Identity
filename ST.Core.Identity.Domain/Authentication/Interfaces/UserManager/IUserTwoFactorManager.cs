using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.UserManager
{
    /// <summary>
    /// Provides methods for managing and retrieving two-factor authentication settings for a user.
    /// </summary>
    /// <typeparam name="TUser">The type of the user, which must inherit from <see cref="IdentityUser"/>.</typeparam>
    public interface IUserTwoFactorManager<TUser>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Asynchronously determines whether two-factor authentication is enabled for the specified user.
        /// </summary>
        /// <param name="user">The user to check two-factor authentication status for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>true</c> if two-factor authentication is enabled; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a list of valid two-factor authentication providers for the specified user.
        /// </summary>
        /// <param name="user">The user to retrieve valid two-factor providers for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of valid two-factor provider names.
        /// </returns>
        Task<IList<string>> GetValidTwoFactorProvidersAsync(TUser user, CancellationToken cancellationToken);
    }
}
