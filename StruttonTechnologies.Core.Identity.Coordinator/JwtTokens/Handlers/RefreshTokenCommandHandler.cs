using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;
using StruttonTechnologies.Core.Identity.Domain.Interfaces.Jwtoken;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to refresh JWT tokens using a refresh token.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class RefreshTokenCommandHandler<TUser, TKey>
        : IRequestHandler<RefreshTokenCommand, TokenResponseDto>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IJwtUserTokenManager<TKey> _tokenManager;
        private readonly IRefreshTokenStore<TKey> _refreshTokenStore;

        public RefreshTokenCommandHandler(
            UserManager<TUser> userManager,
            IJwtUserTokenManager<TKey> tokenManager,
            IRefreshTokenStore<TKey> refreshTokenStore)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
            _tokenManager = tokenManager
                ?? throw new ArgumentNullException(nameof(tokenManager));
            _refreshTokenStore = refreshTokenStore
                ?? throw new ArgumentNullException(nameof(refreshTokenStore));
        }

        public async Task<TokenResponseDto> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Retrieve the refresh token from the store
            Domain.Entities.RefreshToken<TKey>? refreshToken = await _refreshTokenStore.GetAsync(request.RefreshToken, cancellationToken);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                throw new InvalidOperationException("Invalid or expired refresh token.");
            }

            // Check if the refresh token has been revoked
            bool isRevoked = await _tokenManager.IsRefreshTokenRevokedAsync(request.RefreshToken, cancellationToken);
            if (isRevoked)
            {
                throw new InvalidOperationException("Refresh token has been revoked.");
            }

            // Find the user associated with the refresh token
            TUser? user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString()!);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Get user roles for the new token
            IList<string> roles = await _userManager.GetRolesAsync(user);
            string? userName = await _userManager.GetUserNameAsync(user);
            string? email = await _userManager.GetEmailAsync(user);

            // Generate new access token
            string newAccessToken = await _tokenManager.GenerateAccessTokenAsync(
                user.Id,
                userName ?? string.Empty,
                email ?? string.Empty,
                roles,
                cancellationToken);

            // Generate new refresh token (token rotation)
            string newRefreshToken = await _tokenManager.GenerateRefreshTokenAsync(
                user.Id,
                userName ?? string.Empty,
                cancellationToken);

            // Revoke the old refresh token
            await _refreshTokenStore.RevokeAsync(request.RefreshToken, cancellationToken);

            // Get expiration time
            DateTime? expiresAt = await _tokenManager.GetExpirationAsync(newAccessToken);

            return new TokenResponseDto(
                newAccessToken,
                newRefreshToken,
                expiresAt ?? DateTime.UtcNow.AddMinutes(60));
        }
    }
}
