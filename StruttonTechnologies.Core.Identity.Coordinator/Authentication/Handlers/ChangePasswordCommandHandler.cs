using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    /// <summary>
    /// MediatR handler that processes password change requests for authenticated users.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public class ChangePasswordCommandHandler<TUser, TKey>
        : IRequestHandler<ChangePasswordCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public ChangePasswordCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            return user == null
                ? IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." })
                : await _userManager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
                    request.NewPassword);
        }
    }
}
