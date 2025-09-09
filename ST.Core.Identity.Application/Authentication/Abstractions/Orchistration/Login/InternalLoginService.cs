using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Domain.Authentication.Interfaces.UserManager;
using ST.Core.Identity.Domain.Authorization.Interfaces;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System.Security;

namespace ST.Core.Identity.Application.Authentication.Abstractions.Orchistration.Login
{
    public class InternalLoginService : IInternalLoginService
    {
        private readonly IUserLookupManager<IdentityUser> _userLookup;
        private readonly IUserPasswordManager<IdentityUser> _passwordManager;
        private readonly IUserLockoutManager<IdentityUser> _lockoutManager;
        private readonly IUserAuthorizationManager<IdentityUser> _authorizationManager;
        private readonly IJwtUserTokenManager _tokenManager;

        public InternalLoginService(
            IUserLookupManager<IdentityUser> userLookup,
            IUserPasswordManager<IdentityUser> passwordManager,
            IUserLockoutManager<IdentityUser> lockoutManager,
            IUserAuthorizationManager<IdentityUser> authorizationManager,
            IJwtUserTokenManager tokenManager)
        {
            _userLookup = userLookup;
            _passwordManager = passwordManager;
            _lockoutManager = lockoutManager;
            _authorizationManager = authorizationManager;
            _tokenManager = tokenManager;
        }

        public async Task<LoginResponseDto> AuthenticateAsync(InternalLoginRequestDto request, CancellationToken cancellationToken)
        {
            var user = await _userLookup.FindByUsernameAsync(request.UsernameOrEmail, cancellationToken)
                       ?? await _userLookup.FindByEmailAsync(request.UsernameOrEmail, cancellationToken);

            if (user == null || !await _passwordManager.CheckPasswordAsync(user, request.Password, cancellationToken))
                throw new UnauthorizedAccessException("Invalid credentials");

            if (await _lockoutManager.IsLockedOutAsync(user, cancellationToken))
                throw new SecurityException("User is locked out");

            var roles = await _authorizationManager.GetRolesAsync(user, cancellationToken);

            var accessToken = await _tokenManager.GenerateAccessTokenAsync(user.Id, user.UserName, user.Email, roles, cancellationToken);
            var refreshToken = await _tokenManager.GenerateRefreshTokenAsync(user.Id, user.UserName, cancellationToken);

            return new LoginResponseDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresAt: DateTime.UtcNow.AddHours(1),
                UserId: user.Id,
                Username: user.UserName,
                Provider: "Internal",
                IsNewUser: false,
                RequiresTwoFactor: await _userLookup.GetTwoFactorEnabledAsync(user)
            );
        }
    }
}