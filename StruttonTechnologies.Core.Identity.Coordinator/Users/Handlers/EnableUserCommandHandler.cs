using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Users.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to enable a user account by removing lockout.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class EnableUserCommandHandler<TUser, TKey>
        : IRequestHandler<EnableUserCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public EnableUserCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            EnableUserCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." });
            }

            // Remove lockout by setting the end date to null
            IdentityResult result = await _userManager.SetLockoutEndDateAsync(user, null);

            return result;
        }
    }
}
