using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

public sealed class RegenerateMfaRecoveryCodesCommandHandler<TUser, TKey>
    : IRequestHandler<RegenerateMfaRecoveryCodesCommand, MfaRecoveryCodesResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public RegenerateMfaRecoveryCodesCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<MfaRecoveryCodesResultDto> Handle(RegenerateMfaRecoveryCodesCommand request, CancellationToken cancellationToken)
    {
        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return new MfaRecoveryCodesResultDto(false, Array.Empty<string>(), $"User with ID '{request.UserId}' was not found.");
        }

        IEnumerable<string>? codes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        return new MfaRecoveryCodesResultDto(true, (codes ?? Array.Empty<string>()).ToArray());
    }
}
