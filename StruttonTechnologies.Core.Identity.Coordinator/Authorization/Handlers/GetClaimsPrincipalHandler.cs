using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Mapping;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Exceptions;
using StruttonTechnologies.Core.ToolKit.GuardKit;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers
{
    public class GetClaimsPrincipalHandler<TUser> : IRequestHandler<GetClaimsPrincipalQuery, ClaimsPrincipalDto>
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetClaimsPrincipalHandler(UserManager<TUser> userManager)
        {
            ArgumentNullException.ThrowIfNull(userManager);

            _userManager = userManager;
        }

        public async Task<ClaimsPrincipalDto> Handle(
            GetClaimsPrincipalQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            TUser user = Guard.IsNull(await _userManager.FindByIdAsync(request.UserId))
                .ReturnOrThrow(() => new UserNotFoundException(request.UserId));

            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                "Identity.Application",
                ClaimTypes.Name,
                ClaimTypes.Role);

            return identity.ToClaimsPrincipalDto();
        }
    }
}
