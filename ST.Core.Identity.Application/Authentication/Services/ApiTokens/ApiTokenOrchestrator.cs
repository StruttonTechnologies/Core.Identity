using ST.Core.Identity.Application.Authentication.Services.Password;
using ST.Core.Identity.Domain.Authentication.Interfaces.UserManager;
using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Identity.Dtos.Authentication.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Services.ApiTokens
{
    public class ApiTokenOrchestrator : IApiTokenOrchestrator
    {
        private readonly IUserLookupService _userLookup;
        private readonly IPasswordService _password;
        private readonly ILockoutService _lockout;
        private readonly IUserTokenManager _token;

        public ApiTokenOrchestrator(
            IUserLookupService userLookup,
            IPasswordService password,
            ILockoutService lockout,
            IUserTokenManager token)
        {
            _userLookup = userLookup;
            _password = password;
            _lockout = lockout;
            _token = token;
        }

        public async Task<LoginResponseDto> AuthenticateAsync(InternalLoginRequestDto request, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Username);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);

            var user = await _userLookup.FindByUsernameAsync(request.Username, cancellationToken)
                ?? throw new InvalidOperationException("User not found.");

            if (await _lockout.IsLockedOutAsync(user.Id, cancellationToken))
                throw new InvalidOperationException("User is locked out.");

            var isValid = await _password.CheckPasswordAsync(user.Id, request.Password, cancellationToken);
            if (!isValid)
            {
                await _lockout.AccessFailedAsync(user.Id, cancellationToken);
                throw new InvalidOperationException("Invalid credentials.");
            }

            var token = _token.GenerateToken(user.ToClaimsPrincipal());
            var expiresAt = _token.GetExpiration(token);

            return user.ToLoginResponseDto(token, expiresAt ?? DateTime.UtcNow.AddMinutes(15));
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var principal = _token.ValidateToken(refreshToken)
                ?? throw new InvalidOperationException("Invalid refresh token.");

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("UserId claim missing.");

            var token = _token.GenerateToken(principal);
            var expiresAt = _token.GetExpiration(token);

            return new TokenResponseDto(userId, token, expiresAt ?? DateTime.UtcNow.AddMinutes(15));
        }
    }
}
