using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to retrieve all external logins for a user.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class GetUserLoginsQueryHandler<TUser, TKey>
        : IRequestHandler<GetUserLoginsQuery, IList<UserLoginInfo>>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserLoginsQueryHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IList<UserLoginInfo>> Handle(
            GetUserLoginsQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return [];
            }

            return await _userManager.GetLoginsAsync(user);
        }
    }
}
