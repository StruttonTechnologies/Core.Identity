using StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to unlink an external login provider (e.g., Google, Facebook, Microsoft) from an existing user account.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class UnlinkExternalLoginCommandHandler<TUser, TKey>
        : IRequestHandler<UnlinkExternalLoginCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public UnlinkExternalLoginCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            UnlinkExternalLoginCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." });
            }

            return await _userManager.RemoveLoginAsync(user, request.Provider, request.ProviderKey);
        }
    }
}
