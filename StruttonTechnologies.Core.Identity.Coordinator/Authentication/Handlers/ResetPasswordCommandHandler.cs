using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

/// <summary>
/// MediatR handler that processes password reset requests for users.
/// </summary>
public class ResetPasswordCommandHandler<TUser, TKey>
    : IRequestHandler<ResetPasswordCommand, ResetPasswordResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<ResetPasswordResultDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!string.IsNullOrEmpty(request.ConfirmPassword) && request.NewPassword != request.ConfirmPassword)
        {
            return ResetPasswordResultDto.Failure("Password confirmation does not match.");
        }

        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return ResetPasswordResultDto.Failure($"User with ID '{request.UserId}' was not found.");
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded
            ? ResetPasswordResultDto.SuccessResult()
            : ResetPasswordResultDto.Failure(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
