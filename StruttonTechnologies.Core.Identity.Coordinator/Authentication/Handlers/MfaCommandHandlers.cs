using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

public sealed class SetupMfaCommandHandler<TUser, TKey> : IRequestHandler<SetupMfaCommand, MfaSetupResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public SetupMfaCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<MfaSetupResultDto> Handle(SetupMfaCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return new MfaSetupResultDto(false, FailureReason: $"User with ID '{request.UserId}' was not found.");
        }

        string? key = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrWhiteSpace(key))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            key = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        string email = await _userManager.GetEmailAsync(user)
            ?? await _userManager.GetUserNameAsync(user)
            ?? request.UserId;

        string uri = $"otpauth://totp/StruttonTechnologies:{Uri.EscapeDataString(email)}?secret={key}&issuer=StruttonTechnologies&digits=6";

        return new MfaSetupResultDto(true, key, uri);
    }
}
