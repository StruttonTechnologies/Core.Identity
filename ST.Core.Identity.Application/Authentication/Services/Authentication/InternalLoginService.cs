using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Domain.Interfaces.UserManager;
using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Validators;
using System.Security;

namespace ST.Core.Identity.Application.Authentication.Services.Authentication
{
    /// <summary>
    /// Provides internal login authentication services for users.
    /// </summary>
    /// <typeparam name="TUser">The user entity type, which must inherit from <see cref="IdentityUser{TKey}"/> and implement <see cref="IIdentityUserContract{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the user's primary key.</typeparam>
    public class InternalLoginService<TUser, TKey> : IInternalLoginService<TUser, TKey>
        where TUser : IdentityUser<TKey>, IIdentityUserContract<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Provides user lookup operations.
        /// </summary>
        private readonly IUserLookupManager<TUser, TKey> _userLookup;

        /// <summary>
        /// Provides password management operations.
        /// </summary>
        private readonly IUserPasswordManager<TUser, TKey> _passwordManager;

        /// <summary>
        /// Provides lockout management operations.
        /// </summary>
        private readonly IUserLockoutManager<TUser, TKey> _lockoutManager;

        /// <summary>
        /// Provides user authorization and role management operations.
        /// </summary>
        private readonly IUserAuthorizationManager<TUser, TKey> _authorizationManager;

        /// <summary>
        /// Provides JWT token generation operations.
        /// </summary>
        private readonly IJwtUserTokenManager<TKey> _tokenManager;

        /// <summary>
        /// Provides two-factor authentication management operations.
        /// </summary>
        private readonly IUserTwoFactorManager<TUser, TKey> _twoFactorManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalLoginService{TUser, TKey}"/> class.
        /// </summary>
        /// <param name="userLookup">The user lookup manager.</param>
        /// <param name="passwordManager">The password manager.</param>
        /// <param name="lockoutManager">The lockout manager.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="tokenManager">The JWT token manager.</param>
        /// <param name="twoFactorManager">The two-factor authentication manager.</param>
        /// <param name="requestValidator">The login request validator.</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null.</exception>
        public InternalLoginService(
            IUserLookupManager<TUser, TKey> userLookup,
            IUserPasswordManager<TUser, TKey> passwordManager,
            IUserLockoutManager<TUser, TKey> lockoutManager,
            IUserAuthorizationManager<TUser, TKey> authorizationManager,
            IJwtUserTokenManager<TKey> tokenManager,
            IUserTwoFactorManager<TUser, TKey> twoFactorManager,
            IValidator<InternalLoginRequestDto> requestValidator)
        {
            _userLookup = userLookup ?? throw new ArgumentNullException(nameof(userLookup));
            _passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
            _lockoutManager = lockoutManager ?? throw new ArgumentNullException(nameof(lockoutManager));
            _authorizationManager = authorizationManager ?? throw new ArgumentNullException(nameof(authorizationManager));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _twoFactorManager = twoFactorManager ?? throw new ArgumentNullException(nameof(twoFactorManager));
        }


        /// <summary>
        /// Finds a user record by username or email.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The user record.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="usernameOrEmail"/> is null.</exception>
        /// <exception cref="Exception">Thrown if the user is not found.</exception>
        public async Task<TUser> FindUserRecord(string usernameOrEmail, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(usernameOrEmail);
            var user = _userLookup.FindByUsernameAsync(usernameOrEmail, cancellationToken).GetAwaiter().GetResult();

            throw new Exception("User Not Found. Please fill out a registration");
        }

        /// <summary>
        /// Ensures the specified user is not locked out.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
        /// <exception cref="SecurityException">Thrown if the user is locked out.</exception>
        public async Task EnsureUserNotLockedOut(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);

            var isLockedOut = _lockoutManager.IsLockedOutAsync(user, cancellationToken).GetAwaiter().GetResult();

            if (isLockedOut)
                throw new SecurityException($"User '{user.UserName}' is locked out.");
        }

        /// <summary>
        /// Validates the provided password for the specified user.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <param name="password">The password to validate.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> or <paramref name="password"/> is null.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the password is invalid.</exception>
        public async Task EnsureValidPassword(TUser user, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(password);
            var valid = await _passwordManager.CheckPasswordAsync(user, password, cancellationToken);

            if (!valid)
                throw new UnauthorizedAccessException("Invalid password.");
        }

        /// <summary>
        /// Retrieves the roles assigned to the specified user.
        /// </summary>
        /// <param name="user">The user to retrieve roles for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of role names assigned to the user.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
        public async Task<IList<string>> GetUserRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            return await _authorizationManager.GetRolesAsync(user, cancellationToken);
        }

        /// <summary>
        /// Generates an access token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <param name="roles">The roles assigned to the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The generated access token.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
        public async Task<string> GenerateAccessTokenAsync(TUser user, IList<string> roles, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            return await _tokenManager.GenerateAccessTokenAsync(
                user.Id,
                user.UserName,
                user.Email,
                roles,
                cancellationToken);
        }

        /// <summary>
        /// Generates a refresh token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The generated refresh token.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
        public async Task<string> GenerateRefreshTokenAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            return await _tokenManager.GenerateRefreshTokenAsync(
                user.Id,
                user.UserName,
                cancellationToken);
        }

        /// <summary>
        /// Builds a login response DTO for the specified user.
        /// </summary>
        /// <param name="user">The user to build the response for.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="LoginResponseDto"/> containing authentication details for the user.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
        public async Task<LoginResponseDto> BuildLoginResponseAsync(
            TUser user,
            string accessToken,
            string refreshToken,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);

            return new LoginResponseDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresAt: DateTime.UtcNow.AddHours(1),
                Username: user.UserName,
                Provider: "Internal",
                IsNewUser: false,
                RequiresTwoFactor: await _twoFactorManager.GetTwoFactorEnabledAsync(user, cancellationToken)
            );
        }
    }
}