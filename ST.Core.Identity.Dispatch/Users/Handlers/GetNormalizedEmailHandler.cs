using Microsoft.AspNetCore.Identity;
using MediatR;
using ST.Core.Identity.Dispatch.Users.Queries;

namespace ST.Core.Identity.Dispatch.Users.Handlers
{
    public class GetNormalizedEmailHandler<TUser> : IRequestHandler<GetNormalizedEmailQuery, string?>
        where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetNormalizedEmailHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string?> Handle(GetNormalizedEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            return user is null ? null : await _userManager.GetEmailAsync(user);
        }
    }
}