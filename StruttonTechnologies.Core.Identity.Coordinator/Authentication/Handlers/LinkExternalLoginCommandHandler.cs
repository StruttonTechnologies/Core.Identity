using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.ExternalLogins.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to link an external login provider (e.g., Google, Facebook, Microsoft) to an existing user account.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public class LinkExternalLoginCommandHandler<TUser, TKey>
        : IRequestHandler<LinkExternalLoginCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public LinkExternalLoginCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            LinkExternalLoginCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // TODO: Validate the external provider's token before linking
            // This would typically involve calling the provider's token validation endpoint
            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"User with ID '{request.UserId}' not found." });
            }

            UserLoginInfo loginInfo = new UserLoginInfo(
                request.Provider,
                request.ProviderKey,
                request.Provider); // Display name defaults to provider name

            return await _userManager.AddLoginAsync(user, loginInfo);
        }
    }
}
