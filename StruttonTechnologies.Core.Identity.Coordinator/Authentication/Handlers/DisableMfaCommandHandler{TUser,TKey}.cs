using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

public sealed class DisableMfaCommandHandler<TUser, TKey> : IRequestHandler<DisableMfaCommand, MfaVerifyResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public DisableMfaCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<MfaVerifyResultDto> Handle(DisableMfaCommand request, CancellationToken cancellationToken)
    {
        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return new MfaVerifyResultDto(false, $"User with ID '{request.UserId}' was not found.");
        }

        IdentityResult result = await _userManager.SetTwoFactorEnabledAsync(user, false);
        return result.Succeeded
            ? new MfaVerifyResultDto(true)
            : new MfaVerifyResultDto(false, string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
