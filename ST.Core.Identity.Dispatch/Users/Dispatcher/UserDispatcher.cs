using MediatR;
using ST.Core.Identity.Dispatch.Authorization.Queries;
using ST.Core.Identity.Dispatch.Users.Queries;
using ST.Core.Identity.Dispatcher.Contracts.Users;
using ST.Core.Identity.Dtos.Authorization;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.Identity.Dispatch.Users.Dispatcher
{
    public class UserDispatcher : IUserDispatcher
    {
        private readonly IMediator _mediator;

        public UserDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ClaimsPrincipalDto?> GetClaimsPrincipalAsync(string userId)
        {
            return await _mediator.Send(new GetClaimsPrincipalQuery(userId));
        }

        public async Task<string?> GetNormalizedEmailAsync(string userId)
        {
            return await _mediator.Send(new GetNormalizedEmailQuery(userId));
        }
        public async Task<IReadOnlyList<string>> GetUserRolesAsync(string userId)
        {
            var roles = await _mediator.Send(new GetUserRolesQuery(userId));
            return (roles as IReadOnlyList<string>) ?? roles.ToList().AsReadOnly();
        }
        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            return await _mediator.Send(new GetUserProfileQuery(userId));
        }
    }
}
