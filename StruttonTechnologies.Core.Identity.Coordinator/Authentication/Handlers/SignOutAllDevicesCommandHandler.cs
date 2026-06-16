using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

/// <summary>
/// Handles sign-out-all by revoking all refresh tokens and access-token revocations associated with a user.
/// </summary>
/// <typeparam name="TUser">The identity user type.</typeparam>
/// <typeparam name="TKey">The identity key type.</typeparam>
public sealed class SignOutAllDevicesCommandHandler<TUser, TKey> : IRequestHandler<SignOutAllDevicesCommand, SignOutResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;
    private readonly IJwtUserTokenManager<TKey> _tokenManager;

    public SignOutAllDevicesCommandHandler(
        UserManager<TUser> userManager,
        IJwtUserTokenManager<TKey> tokenManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
    }

    public async Task<SignOutResultDto> Handle(SignOutAllDevicesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return SignOutResultDto.Failure($"User with ID '{request.UserId}' was not found.");
        }

        await _tokenManager.RevokeAccessTokensAsync(user.Id, cancellationToken);
        await _tokenManager.RevokeRefreshTokensAsync(user.Id, cancellationToken);

        return SignOutResultDto.SuccessResult();
    }
}
