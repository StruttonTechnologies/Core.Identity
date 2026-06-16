using StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers;

/// <summary>
/// MediatR handler that processes requests to generate JWT access and refresh tokens for a user.
/// </summary>
internal class GenerateTokenCommandHandler<TUser, TKey>
    : IRequestHandler<GenerateTokenCommand, TokenResponseDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;
    private readonly IJwtUserTokenManager<TKey> _tokenManager;

    public GenerateTokenCommandHandler(UserManager<TUser> userManager, IJwtUserTokenManager<TKey> tokenManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
    }

    public async Task<TokenResponseDto> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{request.UserId}' not found.");
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        string userName = await _userManager.GetUserNameAsync(user) ?? string.Empty;
        string email = await _userManager.GetEmailAsync(user) ?? string.Empty;

        string accessToken = await _tokenManager.GenerateAccessTokenAsync(user.Id, userName, email, roles, cancellationToken);
        string refreshToken = await _tokenManager.GenerateRefreshTokenAsync(user.Id, userName, cancellationToken);
        DateTime? accessTokenExpiresAtUtc = await _tokenManager.GetExpirationAsync(accessToken);

        return new TokenResponseDto(
            accessToken,
            refreshToken,
            accessTokenExpiresAtUtc ?? DateTime.UtcNow.AddMinutes(15),
            DateTime.UtcNow.AddDays(7));
    }
}
