using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Users.Commands;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// MediatR handler that processes user creation requests.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public class CreateUserCommandHandler<TUser, TKey>
        : IRequestHandler<CreateUserCommand, IdentityResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public CreateUserCommandHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser user = new TUser();

            await _userManager.SetUserNameAsync(user, request.UserName);

            if (!string.IsNullOrEmpty(request.Email))
            {
                await _userManager.SetEmailAsync(user, request.Email);
            }

            return string.IsNullOrEmpty(request.Password)
                ? await _userManager.CreateAsync(user)
                : await _userManager.CreateAsync(user, request.Password);
        }
    }
}
