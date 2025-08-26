using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    /// <summary>
    /// Provides methods for looking up user information.
    /// </summary>
    public interface IUserLookup<TUser>
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        Task<TUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An enumerable collection of user entities.</returns>
        Task<IEnumerable<TUser>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        Task<TUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        Task<TUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
