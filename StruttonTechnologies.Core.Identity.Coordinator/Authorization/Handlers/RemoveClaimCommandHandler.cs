using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers;

/// <summary>
/// Removes a claim from a user.
/// </summary>
public sealed class RemoveClaimCommandHandler<TUser, TKey> : IRequestHandler<RemoveClaimCommand, IdentityResult>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly UserManager<TUser> _userManager;

    public RemoveClaimCommandHandler(UserManager<TUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IdentityResult> Handle(RemoveClaimCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        TUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{request.UserId}' was not found." });
        }

        return await _userManager.RemoveClaimAsync(user, new Claim(request.ClaimType, request.ClaimValue));
    }
}
