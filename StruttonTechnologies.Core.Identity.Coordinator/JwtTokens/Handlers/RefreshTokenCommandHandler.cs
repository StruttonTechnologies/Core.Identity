using StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers;

/// <summary>
/// MediatR handler that processes requests to refresh JWT tokens using a refresh token.
/// </summary>
internal class RefreshTokenCommandHandler<TUser, TKey>
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResultDto>
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
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        _refreshTokenStore = refreshTokenStore ?? throw new ArgumentNullException(nameof(refreshTokenStore));
    }

    public async Task<RefreshTokenResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Domain.Entities.RefreshToken<TKey>? refreshToken = await _refreshTokenStore.GetAsync(request.RefreshToken, cancellationToken);
        if (refreshToken is null || !refreshToken.IsActive)
        {
            return RefreshTokenResultDto.Failure("Invalid or expired refresh token.");
        }

        TUser? user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString()!);
        if (user is null)
        {
            return RefreshTokenResultDto.Failure("User was not found.");
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        string userName = await _userManager.GetUserNameAsync(user) ?? refreshToken.Username;
        string email = await _userManager.GetEmailAsync(user) ?? string.Empty;

        string accessToken = await _tokenManager.GenerateAccessTokenAsync(user.Id, userName, email, roles, cancellationToken);
        string newRefreshToken = await _tokenManager.GenerateRefreshTokenAsync(user.Id, userName, cancellationToken);

        await _refreshTokenStore.RevokeAsync(request.RefreshToken, cancellationToken);

        DateTime? accessTokenExpiresAtUtc = await _tokenManager.GetExpirationAsync(accessToken);

        return RefreshTokenResultDto.SuccessResult(accessToken, newRefreshToken, accessTokenExpiresAtUtc);
    }
}
