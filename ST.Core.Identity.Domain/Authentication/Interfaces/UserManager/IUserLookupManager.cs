using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.UserManager
{
    /// <summary>
    /// Provides methods for looking up user information.
    /// </summary>
    /// <typeparam name="TUser">The type of the user entity.</typeparam>
    public interface IUserLookupManager<TUser>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Finds a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The user if found; otherwise, <c>null</c>.</returns>
        Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken);

        /// <summary>
        /// Finds a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The user if found; otherwise, <c>null</c>.</returns>
        Task<TUser?> FindByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Finds a user by their external login information.
        /// </summary>
        /// <param name="loginProvider">The login provider (e.g., Google, Facebook).</param>
        /// <param name="providerKey">The unique key provided by the login provider.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The user if found; otherwise, <c>null</c>.</returns>
        Task<TUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);

        /// <summary>
        /// Finds a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The user if found; otherwise, <c>null</c>.</returns>
        Task<TUser?> FindByUsernameAsync(string username, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An enumerable collection of all users.</returns>
        Task<IEnumerable<TUser>> GetAllUsersAsync(CancellationToken cancellationToken);
    }
}
