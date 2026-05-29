using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Coordinator
{
    /// <summary>
    /// Coordinates authorization queries through MediatR.
    /// </summary>
    public class AuthorizationCoordinator : IAuthorizationCoordinator
    {
        private readonly IMediator _mediator;

        public AuthorizationCoordinator(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ClaimsPrincipalDto?> GetClaimsPrincipalAsync(string userId)
        {
            return await _mediator.Send(new GetClaimsPrincipalQuery(userId));
        }
    }
}
