using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    /// <summary>
    /// MediatR handler that processes email confirmation requests for users.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public class ConfirmEmailCommandHandler<TUser, TKey>
        : IRequestHandler<ConfirmEmailCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public ConfirmEmailCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            return user == null
                ? IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." })
                : await _userManager.ConfirmEmailAsync(user, request.Token);
        }
    }
}
