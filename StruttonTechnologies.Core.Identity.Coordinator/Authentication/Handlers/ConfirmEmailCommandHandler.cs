using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;

/// <summary>
/// MediatR handler that processes email confirmation requests for users.
/// </summary>
/// <typeparam name="TUser">The type of the user.</typeparam>
/// <typeparam name="TKey">The type of the user's key.</typeparam>
public class ConfirmEmailCommandHandler<TUser, TKey>
    : IRequestHandler<ConfirmEmailCommand, ConfirmEmailResultDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<ConfirmEmailResultDto> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return ConfirmEmailResultDto.Failure($"User with ID '{request.UserId}' was not found.");
        }

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, request.Token);
        return result.Succeeded
            ? ConfirmEmailResultDto.SuccessResult()
            : ConfirmEmailResultDto.Failure(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
