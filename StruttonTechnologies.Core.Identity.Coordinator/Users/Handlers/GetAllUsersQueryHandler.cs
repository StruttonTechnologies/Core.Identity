using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// Handles requests to retrieve all users in the system.
    /// </summary>
    /// <typeparam name="TUser">The user type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public class GetAllUsersQueryHandler<TUser, TKey>
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

        public Task<IEnumerable<TUser>> Handle(
            GetAllUsersQuery<TUser> request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            IEnumerable<TUser> users = _userManager.Users.ToList();

            return Task.FromResult(users);
        }
    }
}
