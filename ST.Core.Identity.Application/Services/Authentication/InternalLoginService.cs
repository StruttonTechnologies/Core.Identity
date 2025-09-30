using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Identity.Exceptions;
using ST.Core.Validators;
using ST.Core.Validators.Gaurds;
using System.Security;

namespace ST.Core.Identity.Application.Services.Authentication
{
    public class InternalLoginService<TUser, TKey>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        protected readonly ILogger<InternalLoginService<TUser, TKey>> _logger;
        private readonly IJwtUserTokenManager<TKey> _tokenManager;

        public InternalLoginService(
            UserManager<TUser> userManager,
            IJwtUserTokenManager<TKey> tokenManager,
            ILogger<InternalLoginService<TUser, TKey>> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LoginResponseDto> InternalLoginWorkFlow(
            InternalLoginRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(request);

                //Get User Recored from Datatbase
                var user = await FindUserRecordAsync(request.UserName, cancellationToken);

                // Check if User is Locked Out
                await EnsureUserNotLockedOutAsync(user);

                // Validate the users Password
                await EnsureValidPasswordAsync(user, request.Password);

                // Get User Roles
                var roles = await _userManager.GetRolesAsync(user);

                // Generate Tokens
                var accessToken = await GenerateAccessTokenAsync(user, roles, cancellationToken);
                var refreshToken = await GenerateRefreshTokenAsync(user, cancellationToken);

                // Build and return Dto for Response
                return await BuildLoginResponseAsync(user, accessToken, refreshToken, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Username}", request?.UserName);
                throw;
            }
        }

        protected internal virtual async Task<TUser> FindUserRecordAsync(string usernameOrEmail, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(usernameOrEmail);

            var user = await _userManager.FindByNameAsync(usernameOrEmail)
                       ?? await _userManager.FindByEmailAsync(usernameOrEmail);

            return user ?? throw new UserNotFoundException("User Not Found. Please fill out a registration.");
        }

        protected internal virtual async Task EnsureUserNotLockedOutAsync(TUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (await _userManager.IsLockedOutAsync(user))
                throw new SecurityException($"User '{user.UserName}' is locked out.");
        }

        protected internal virtual async Task EnsureValidPasswordAsync(TUser user, string password)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(password);

            if (!await _userManager.CheckPasswordAsync(user, password))
                throw new UnauthorizedAccessException("Invalid password.");
        }

        protected internal virtual async Task<string> GenerateAccessTokenAsync(TUser user, IList<string> roles, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(user.UserName, nameof(user.UserName));
            ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));
            ArgumentExceptionExtensions.ThrowIfIdInvalid(user.Id);

            return await _tokenManager.GenerateAccessTokenAsync(
                user.Id,
                user.UserName,
                user.Email,
                roles,
                cancellationToken);
        }

        protected internal virtual async Task<string> GenerateRefreshTokenAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(user.UserName, nameof(user.UserName));
            ArgumentExceptionExtensions.ThrowIfIdInvalid(user.Id);

            return await _tokenManager.GenerateRefreshTokenAsync(
                user.Id,
                user.UserName,
                cancellationToken);
        }

        protected internal virtual async Task<LoginResponseDto> BuildLoginResponseAsync(
            TUser user,
            string accessToken,
            string refreshToken,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(user.UserName, nameof(user.UserName));
            ArgumentException.ThrowIfNullOrWhiteSpace(accessToken, nameof(accessToken));
            ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken, nameof(refreshToken));

            return new LoginResponseDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresAt: DateTime.UtcNow.AddHours(1),
                Username: user.UserName,
                Provider: "Internal",
                IsNewUser: false,
                RequiresTwoFactor: await _userManager.GetTwoFactorEnabledAsync(user));
        }
    }
}