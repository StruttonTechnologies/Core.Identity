using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    /// <summary>
    /// Provides methods to update and manage user information.
    /// </summary>
    /// <typeparam name="TUser">The user entity type.</typeparam>
    public interface IUserUpdater<TUser>
        where TUser : class
    {
        /// <summary>
        /// Updates the specified user asynchronously.
        /// </summary>
        Task<TUser> UpdateAsync(TUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the password for the specified user asynchronously.
        /// </summary>
        Task<bool> UpdatePasswordAsync(TUser user, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the email for the specified user asynchronously.
        /// </summary>
        Task<bool> UpdateEmailAsync(TUser user, string newEmail, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the username for the specified user asynchronously.
        /// </summary>
        Task<bool> UpdateUsernameAsync(TUser user, string newUsername, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified user asynchronously.
        /// </summary>
        Task<bool> DeleteAsync(TUser user, CancellationToken cancellationToken = default);
    }
}