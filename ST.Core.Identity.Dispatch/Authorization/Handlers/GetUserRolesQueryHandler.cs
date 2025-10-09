using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Dispatch.Users.Queries;
using ST.Core.Registration.Attributes;

namespace ST.Core.Identity.Dispatch.Authorization.Handlers
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class GetUserRolesQueryHandler<TUser> : IRequestHandler<GetUserRolesQuery, IReadOnlyList<string>>
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserRolesQueryHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Array.Empty<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    }
}