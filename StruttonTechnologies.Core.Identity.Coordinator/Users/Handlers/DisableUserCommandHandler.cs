using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to disable a user account by setting lockout.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class DisableUserCommandHandler<TUser, TKey>
        : IRequestHandler<DisableUserCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public DisableUserCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            DisableUserCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." });
            }

            // Enable lockout for the user
            IdentityResult enableLockoutResult = await _userManager.SetLockoutEnabledAsync(user, true);

            if (!enableLockoutResult.Succeeded)
            {
                return enableLockoutResult;
            }

            // Set lockout end date to a far future date to effectively disable the account
            IdentityResult lockoutResult = await _userManager.SetLockoutEndDateAsync(
                user,
                DateTimeOffset.MaxValue);

            return lockoutResult;
        }
    }
}
