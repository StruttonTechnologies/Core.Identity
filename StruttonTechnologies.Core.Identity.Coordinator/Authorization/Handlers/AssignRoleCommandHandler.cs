using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to assign a role to a user.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class AssignRoleCommandHandler<TUser, TKey>
        : IRequestHandler<AssignRoleCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public AssignRoleCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            AssignRoleCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." });
            }

            return await _userManager.AddToRoleAsync(user, request.RoleName);
        }
    }
}
