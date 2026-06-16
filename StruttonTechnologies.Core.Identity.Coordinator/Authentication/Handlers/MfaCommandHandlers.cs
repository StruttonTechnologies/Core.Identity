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

        string email = await _userManager.GetEmailAsync(user) ?? await _userManager.GetUserNameAsync(user) ?? request.UserId;
        string uri = $"otpauth://totp/StruttonTechnologies:{Uri.EscapeDataString(email)}?secret={key}&issuer=StruttonTechnologies&digits=6";
        return new MfaSetupResultDto(true, key, uri);
    }
}

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
