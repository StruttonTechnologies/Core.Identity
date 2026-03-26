using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.Coordinator.Users.Queries;
using StruttonTechnologies.Core.ToolKit.GuardKit;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    public class GetNormalizedEmailHandler<TUser> : IRequestHandler<GetNormalizedEmailQuery, string?>
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetNormalizedEmailHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string?> Handle(
            GetNormalizedEmailQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            return await Guard.IsNull(await _userManager.FindByIdAsync(request.UserId))
                .ReturnAsync(
                    matchedReturn: null,
                    notMatchedFactory: user => _userManager.GetEmailAsync(user));
        }
    }
}
