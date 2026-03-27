using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Coordinator
{
    public class UserCoordinator : IUserCoordinator
    {
        private readonly IMediator _mediator;

        public UserCoordinator(IMediator mediator)
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
            IList<string> roles = await _mediator.Send(new GetUserRolesQuery(userId));
            return (roles as IReadOnlyList<string>) ?? roles.ToList().AsReadOnly();
        }

        public async Task<UserProfileResult> GetUserProfileAsync(string userId)
        {
            return await _mediator.Send(new GetUserProfileQuery(userId));
        }
    }
}
