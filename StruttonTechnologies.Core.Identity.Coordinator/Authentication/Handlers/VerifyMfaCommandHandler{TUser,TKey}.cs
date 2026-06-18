using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

public sealed class VerifyMfaCommandHandler<TUser, TKey> : IRequestHandler<VerifyMfaCommand, MfaVerifyResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public VerifyMfaCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<MfaVerifyResultDto> Handle(VerifyMfaCommand request, CancellationToken cancellationToken)
    {
        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return new MfaVerifyResultDto(false, $"User with ID '{request.UserId}' was not found.");
        }

        bool valid = await _userManager.VerifyTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultAuthenticatorProvider,
            request.Code);

        if (!valid)
        {
            return new MfaVerifyResultDto(false, "Invalid MFA code.");
        }

        IdentityResult result = await _userManager.SetTwoFactorEnabledAsync(user, true);
        return result.Succeeded
            ? new MfaVerifyResultDto(true)
            : new MfaVerifyResultDto(false, string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
