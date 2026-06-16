using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

/// <summary>
/// MediatR handler that processes forgot-password requests and generates password-reset tokens.
/// </summary>
public class ForgotPasswordCommandHandler<TUser, TKey>
    : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public ForgotPasswordCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<ForgotPasswordResultDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        TUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            return ForgotPasswordResultDto.SuccessResult();
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return ForgotPasswordResultDto.SuccessResult(token);
    }
}
