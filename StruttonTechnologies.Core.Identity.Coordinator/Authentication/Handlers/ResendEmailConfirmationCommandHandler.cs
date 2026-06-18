using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

/// <summary>
/// Generates an email-confirmation token for an existing user.
/// </summary>
public sealed class ResendEmailConfirmationCommandHandler<TUser, TKey>
    : IRequestHandler<ResendEmailConfirmationCommand, ResendEmailConfirmationResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public ResendEmailConfirmationCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<ResendEmailConfirmationResultDto> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        TUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return ResendEmailConfirmationResultDto.SuccessResult();
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return ResendEmailConfirmationResultDto.SuccessResult(token);
    }
}
