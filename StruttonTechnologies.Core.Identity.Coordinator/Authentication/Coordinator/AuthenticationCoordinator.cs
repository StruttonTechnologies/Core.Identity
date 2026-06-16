using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Coordinator;

/// <summary>
/// Coordinates authentication commands through MediatR.
/// </summary>
public class AuthenticationCoordinator : IAuthenticationCoordinator
{
    private readonly IMediator _mediator;

    public AuthenticationCoordinator(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<RegistrationResultDto> RegisterAsync(
        string email,
        string password,
        string displayName,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RegisterUserCommand(email, password, displayName), cancellationToken);
    }

    public async Task<AuthenticationResultDto> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new AuthenticateUserCommand(email, password), cancellationToken);
    }

    public async Task<RefreshTokenResultDto> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RefreshAuthenticationTokenCommand(refreshToken), cancellationToken);
    }

    public async Task<SignOutResultDto> SignOutAsync(
        string? accessToken,
        string? refreshToken,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new SignOutCommand(accessToken, refreshToken), cancellationToken);
    }

    public async Task<SignOutResultDto> SignOutAllDevicesAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new SignOutAllDevicesCommand(userId), cancellationToken);
    }
}
