using ST.Core.Identity.Application.Authentication.Abstractions.Orchistration.Login;
using ST.Core.Identity.Domain.Authentication.Interfaces.Token;
using ST.Core.Identity.Domain.Authentication.Interfaces.UserManager;
using ST.Core.Identity.Domain.Authorization.Interfaces;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System.Security;

namespace ST.Core.Identity.Application.Authentication.Abstractions.Orchestration.Login
{
    /// <summary>
    /// Provides internal login authentication services for users.
    /// </summary>
    /// <typeparam name="TUser">The user entity type implementing <see cref="IIdentityUserContract"/>.</typeparam>
    /// 
    public class InternalLoginService<TUser> : IInternalLoginService<TUser>
        where TUser : class, IIdentityUserContract
    {
        private readonly IUserLookupManager<TUser> _userLookup;
        private readonly IUserPasswordManager<TUser> _passwordManager;
        private readonly IUserLockoutManager<TUser> _lockoutManager;
        private readonly IUserAuthorizationManager<TUser> _authorizationManager;
        private readonly IJwtUserTokenManager _tokenManager;
        private readonly IUserTwoFactorManager<TUser> _twoFactorManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalLoginService{TUser}"/> class.
        /// </summary>
        /// <param name="userLookup">The user lookup manager.</param>
        /// <param name="passwordManager">The user password manager.</param>
        /// <param name="lockoutManager">The user lockout manager.</param>
        /// <param name="authorizationManager">The user authorization manager.</param>
        /// <param name="tokenManager">The JWT user token manager.</param>
        /// <param name="twoFactorManager">The user two-factor manager.</param>
        public InternalLoginService(
            IUserLookupManager<TUser> userLookup,
            IUserPasswordManager<TUser> passwordManager,
            IUserLockoutManager<TUser> lockoutManager,
            IUserAuthorizationManager<TUser> authorizationManager,
            IJwtUserTokenManager tokenManager,
            IUserTwoFactorManager<TUser> twoFactorManager)
        {
            _userLookup = userLookup;
            _passwordManager = passwordManager;
            _lockoutManager = lockoutManager;
            _authorizationManager = authorizationManager;
            _tokenManager = tokenManager;
            _twoFactorManager = twoFactorManager;
        }

        /// <summary>
        /// Authenticates a user using the provided internal login request.
        /// </summary>
        /// <remarks>
        /// This service orchestrates internal login flows using a generic user contract.
        /// It supports both internal and external identity models via IIdentityUserContract,
        /// enabling flexible token generation and role resolution.
        /// </remarks>
        /// <param name="request">The internal login request data.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A <see cref="LoginResponseDto"/> representing the result of the authentication attempt.
        /// </returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when credentials are invalid.</exception>
        /// <exception cref="SecurityException">Thrown when the user is locked out.</exception>
        public async Task<LoginResponseDto> AuthenticateAsync(InternalLoginRequestDto request, CancellationToken cancellationToken)
        {
            var user = await _userLookup.FindByUsernameAsync(request.UsernameOrEmail, cancellationToken)
                       ?? await _userLookup.FindByEmailAsync(request.UsernameOrEmail, cancellationToken);

            if (user == null || !await _passwordManager.CheckPasswordAsync(user, request.Password, cancellationToken))
                throw new UnauthorizedAccessException("Invalid credentials");

            if (await _lockoutManager.IsLockedOutAsync(user, cancellationToken))
                throw new SecurityException("User is locked out");

            var roles = await _authorizationManager.GetRolesAsync(user, cancellationToken);

            var accessToken = await _tokenManager.GenerateAccessTokenAsync(
                user.Id.ToString(), user.UserName, user.Email, roles, cancellationToken);

            var refreshToken = await _tokenManager.GenerateRefreshTokenAsync(
                user.Id.ToString(), user.UserName, cancellationToken);

            return new LoginResponseDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresAt: DateTime.UtcNow.AddHours(1),
                UserId: user.Id,
                Username: user.UserName,
                Provider: "Internal",
                IsNewUser: false,
                RequiresTwoFactor: await _twoFactorManager.GetTwoFactorEnabledAsync(user, cancellationToken)
            );
        }
    }
}