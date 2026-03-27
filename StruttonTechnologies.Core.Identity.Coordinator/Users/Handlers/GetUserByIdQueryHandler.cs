using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.ExternalLogins;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    /// <summary>
    /// Handles requests to retrieve a user by identifier.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system.</typeparam>
    /// <typeparam name="TKey">The type used for user keys.</typeparam>
    public class GetUserByIdQueryHandler<TUser, TKey>
        : IRequestHandler<GetUserByIdQuery, UserDetailResult>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserByIdQueryHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UserDetailResult> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            ArgumentNullException.ThrowIfNull(user);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<System.Security.Claims.Claim> claims = await _userManager.GetClaimsAsync(user);
            IList<UserLoginInfo> logins = await _userManager.GetLoginsAsync(user);

            return new UserDetailResult(
                user.Id?.ToString() ?? string.Empty,
                user.Email ?? string.Empty,
                user.EmailConfirmed,
                true,
                roles.ToArray(),
                claims.Select(c => new ClaimDto(c.Type, c.Value)).ToArray(),
                logins.Select(l => new ExternalLoginInfoDto(
                    l.LoginProvider,
                    l.ProviderKey,
                    l.ProviderDisplayName ?? l.LoginProvider)).ToArray());
        }
    }
}
