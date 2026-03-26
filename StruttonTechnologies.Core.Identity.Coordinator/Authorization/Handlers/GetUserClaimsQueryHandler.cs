using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Queries;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to retrieve claims for a user.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class GetUserClaimsQueryHandler<TUser, TKey>
        : IRequestHandler<GetUserClaimsQuery, IList<Claim>>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserClaimsQueryHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IList<Claim>> Handle(
            GetUserClaimsQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return [];
            }

            return await _userManager.GetClaimsAsync(user);
        }
    }
}
