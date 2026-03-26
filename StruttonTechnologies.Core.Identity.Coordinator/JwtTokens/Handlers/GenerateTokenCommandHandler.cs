using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to generate JWT access and refresh tokens for a user.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class GenerateTokenCommandHandler<TUser, TKey>
        : IRequestHandler<GenerateTokenCommand, TokenResponseDto>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IJwtUserTokenManager<TKey> _tokenManager;

        public GenerateTokenCommandHandler(
            UserManager<TUser> userManager,
            IJwtUserTokenManager<TKey> tokenManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
            _tokenManager = tokenManager
                ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        public async Task<TokenResponseDto> Handle(
            GenerateTokenCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                throw new InvalidOperationException($"User with ID '{request.UserId}' not found.");
            }

            // Get user claims and roles
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            string? userName = await _userManager.GetUserNameAsync(user);
            string? email = await _userManager.GetEmailAsync(user);

            // Generate access token
            string accessToken = await _tokenManager.GenerateAccessTokenAsync(
                user.Id,
                userName ?? string.Empty,
                email ?? string.Empty,
                roles,
                cancellationToken);

            // Generate refresh token
            string refreshToken = await _tokenManager.GenerateRefreshTokenAsync(
                user.Id,
                userName ?? string.Empty,
                cancellationToken);

            // Get expiration time
            DateTime? expiresAt = await _tokenManager.GetExpirationAsync(accessToken);

            return new TokenResponseDto(
                accessToken,
                refreshToken,
                expiresAt ?? DateTime.UtcNow.AddMinutes(60));
        }
    }
}
