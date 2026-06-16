using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

/// <summary>
/// Handles sign-out by revoking the provided access and refresh tokens.
/// </summary>
/// <typeparam name="TKey">The identity key type.</typeparam>
public sealed class SignOutHandler<TKey> : IRequestHandler<SignOutCommand, SignOutResultDto>
    where TKey : IEquatable<TKey>
{
    private readonly ITokenOrchestration<TKey> _tokenOrchestration;

    public SignOutHandler(ITokenOrchestration<TKey> tokenOrchestration)
    {
        _tokenOrchestration = tokenOrchestration
            ?? throw new ArgumentNullException(nameof(tokenOrchestration));
    }

    public async Task<SignOutResultDto> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.AccessToken) && string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return SignOutResultDto.Failure("An access token or refresh token is required.");
        }

        if (!string.IsNullOrWhiteSpace(request.AccessToken))
        {
            await _tokenOrchestration.RevokeAccessTokenAsync(request.AccessToken, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            await _tokenOrchestration.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
        }

        return SignOutResultDto.SuccessResult();
    }
}
