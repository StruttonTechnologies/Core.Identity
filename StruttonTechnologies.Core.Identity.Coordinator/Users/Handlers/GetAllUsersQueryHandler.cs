using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using StruttonTechnologies.Core.Identity.Coordinator.Users.Queries;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to retrieve all users.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class GetAllUsersQueryHandler<TUser, TKey>
        : IRequestHandler<GetAllUsersQuery<TUser>, IEnumerable<TUser>>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IEnumerable<TUser>> Handle(
            GetAllUsersQuery<TUser> request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Return all users from the UserManager
            return await _userManager.Users.ToListAsync(cancellationToken);
        }
    }
}
