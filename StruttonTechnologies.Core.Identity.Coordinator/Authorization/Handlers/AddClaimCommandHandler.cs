using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to add a claim to a user.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class AddClaimCommandHandler<TUser, TKey>
        : IRequestHandler<AddClaimCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public AddClaimCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            AddClaimCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." });
            }

            Claim claim = new Claim(request.ClaimType, request.ClaimValue);

            return await _userManager.AddClaimAsync(user, claim);
        }
    }
}
