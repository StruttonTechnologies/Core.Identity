using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Queries
{
    /// <summary>
    /// Query to retrieve a ClaimsPrincipalDto for a given user ID.
    /// </summary>
    public sealed record GetClaimsPrincipalQuery(string UserId) : IRequest<ClaimsPrincipalDto>;
}
