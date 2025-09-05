using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    /// <summary>
    /// Provides authentication-related operations for a user.
    /// </summary>
    /// <typeparam name="TUser">The type of the user, derived from <see cref="IdentityUser"/>.</typeparam>
    public interface IUserAuthenticationService<TUser>
        where TUser : IdentityUser
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

        /// <summary>
        /// Generates a password reset token for the user.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The generated password reset token.</returns>
        Task<string> GeneratePasswordResetTokenAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Generates a user token for a specific purpose.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <param name="purpose">The purpose of the token.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The generated user token.</returns>
        Task<string> GenerateUserTokenAsync(TUser user, string tokenProvider, string purpose, CancellationToken cancellationToken);

        /// <summary>
        /// Generates a two-factor authentication token for the user using the specified provider.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <param name="provider">The two-factor provider.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The generated two-factor token.</returns>
        Task<string> GenerateTwoFactorTokenAsync(TUser user, string provider, CancellationToken cancellationToken);

        /// <summary>
        /// Generates an email confirmation token for the user.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The generated email confirmation token.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Confirms the user's email using the provided token.
        /// </summary>
        /// <param name="user">The user whose email will be confirmed.</param>
        /// <param name="token">The email confirmation token.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>True if the email was confirmed; otherwise, false.</returns>
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string token, CancellationToken cancellationToken);

        /// <summary>
        /// Checks if the user's email is confirmed.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>True if the email is confirmed; otherwise, false.</returns>
        Task<bool> IsEmailConfirmedAsync(TUser user, CancellationToken cancellationToken);
    }
}
