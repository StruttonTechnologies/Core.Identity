using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Users.Queries;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to retrieve a user by email address.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class GetUserByEmailQueryHandler<TUser, TKey>
        : IRequestHandler<GetUserByEmailQuery<TUser>, TUser?>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserByEmailQueryHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<TUser?> Handle(
            GetUserByEmailQuery<TUser> request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            return await _userManager.FindByEmailAsync(request.Email);
        }
    }
}
