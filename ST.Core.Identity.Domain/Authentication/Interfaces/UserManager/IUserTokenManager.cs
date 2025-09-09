using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.UserManager
{
    public interface IUserTokenManager<TUser>
        where TUser : IdentityUser
    {
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
        /// Generates a password reset token for the user.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The generated password reset token.</returns>
        Task<string> GeneratePasswordResetTokenAsync(TUser user, CancellationToken cancellationToken);
    }
}
