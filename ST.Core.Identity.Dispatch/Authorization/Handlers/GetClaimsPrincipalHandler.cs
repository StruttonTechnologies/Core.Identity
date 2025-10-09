using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Dispatch.Authorization.Queries;
using ST.Core.Identity.Dispatch.Users.Mapping;
using ST.Core.Identity.Dtos.Authorization;
using ST.Core.Registration.Attributes;
using System.Security.Claims;

namespace ST.Core.Identity.Dispatch.Authorization.Handlers
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class GetClaimsPrincipalHandler<TUser> : IRequestHandler<GetClaimsPrincipalQuery, ClaimsPrincipalDto>
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetClaimsPrincipalHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipalDto> Handle(GetClaimsPrincipalQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return null!;

            var identity = new ClaimsIdentity(
                await _userManager.GetClaimsAsync(user),
                "Identity.Application",
                ClaimTypes.Name,
                ClaimTypes.Role
            );

            return identity.ToClaimsPrincipalDto();
        }
    }
}