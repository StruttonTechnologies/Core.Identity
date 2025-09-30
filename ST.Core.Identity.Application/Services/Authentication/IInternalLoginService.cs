using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Services.Authentication
{
    /// <summary>
    /// Defines a service for handling internal user login authentication.
    /// </summary>
    /// <typeparam name="TUser">The user entity type.</typeparam>
    /// <typeparam name="TKey">The type of the user's primary key.</typeparam>
    public interface IInternalLoginService<TUser, TKey>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Finds a user record by username or email.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The user record if found; otherwise, null.</returns>
        Task<TUser> FindUserRecord(string usernameOrEmail, CancellationToken cancellationToken);

        /// <summary>
        /// Ensures the user is not locked out.
        /// Throws an exception if the user is locked out.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EnsureUserNotLockedOut(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Ensures the provided password is valid for the user.
        /// Throws an exception if the password is invalid.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <param name="password">The password to validate.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EnsureValidPassword(TUser user, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the roles assigned to the user.
        /// </summary>
        /// <param name="user">The user to retrieve roles for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of role names assigned to the user.</returns>
        Task<IList<string>> GetUserRolesAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Authenticates the user and generates login response data.
        /// </summary>
        /// <param name="request">The login request containing credentials.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="LoginResponseDto"/> representing the result of the authentication attempt.</returns>
        Task<LoginResponseDto> LoginAsync(InternalLoginRequestDto request, CancellationToken cancellationToken);

        /// <summary>
        /// Generates an access token for the user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <param name="roles">The roles assigned to the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The generated access token as a string.</returns>
        Task<string> GenerateAccessTokenAsync(TUser user, IList<string> roles, CancellationToken cancellationToken);

        /// <summary>
        /// Generates a refresh token for the user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The generated refresh token as a string.</returns>
        Task<string> GenerateRefreshTokenAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Builds the login response DTO for the user.
        /// </summary>
        /// <param name="user">The user for whom to build the response.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="LoginResponseDto"/> containing the login response data.</returns>
        Task<LoginResponseDto> BuildLoginResponseAsync(TUser user, string accessToken, string refreshToken, CancellationToken cancellationToken);
    }
}
