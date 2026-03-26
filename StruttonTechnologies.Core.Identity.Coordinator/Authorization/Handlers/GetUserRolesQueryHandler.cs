using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Authorization.Queries;
using StruttonTechnologies.Core.ToolKit.GuardKit;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Handlers
{
    public class GetUserRolesQueryHandler<TUser> : IRequestHandler<GetUserRolesQuery, IList<string>>
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserRolesQueryHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            return await Guard.IsNull(await _userManager.FindByIdAsync(request.UserId))
                .ReturnAsync(
                    matchedReturn: Array.Empty<string>(),
                    notMatchedFactory: async user => await _userManager.GetRolesAsync(user));
        }
    }
}
