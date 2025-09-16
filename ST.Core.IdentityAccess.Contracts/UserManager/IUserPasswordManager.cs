using Microsoft.AspNetCore.Identity;

namespace ST.Core.IdentityAccess.Contracts.UserManager
{
    public interface IUserPasswordManager<in TUser> 
        where TUser : class
    {
        /// <summary>
        /// Checks if the specified password is valid for the given user.
        /// </summary>
        /// <param name="user">The user to check the password for.</param>
        /// <param name="password">The password to verify.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        Task<bool> CheckPasswordAsync(TUser user, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Changes the user's password from the current password to a new password.
        /// </summary>
        /// <param name="user">The user whose password will be changed.</param>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a password to the user if one does not already exist.
        /// </summary>
        /// <param name="user">The user to add the password to.</param>
        /// <param name="password">The password to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<IdentityResult> AddPasswordAsync(TUser user, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Removes the password from the user.
        /// </summary>
        /// <param name="user">The user whose password will be removed.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<IdentityResult> RemovePasswordAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Resets the user's password using a reset token.
        /// </summary>
        /// <param name="user">The user whose password will be reset.</param>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword, CancellationToken cancellationToken); 
    }
}
