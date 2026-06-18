using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

/// <summary>
/// MediatR handler that processes password change requests for authenticated users.
/// </summary>
/// <typeparam name="TUser">The type of the user.</typeparam>
/// <typeparam name="TKey">The type of the user's key.</typeparam>
public class ChangePasswordCommandHandler<TUser, TKey>
    : IRequestHandler<ChangePasswordCommand, ChangePasswordResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<ChangePasswordResultDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!string.IsNullOrEmpty(request.ConfirmPassword) && request.NewPassword != request.ConfirmPassword)
        {
            return ChangePasswordResultDto.Failure("Password confirmation does not match.");
        }

        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return ChangePasswordResultDto.Failure($"User with ID '{request.UserId}' was not found.");
        }

        IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded
            ? ChangePasswordResultDto.SuccessResult()
            : ChangePasswordResultDto.Failure(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
