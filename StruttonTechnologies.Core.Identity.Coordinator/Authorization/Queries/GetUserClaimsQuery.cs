using System.Security.Claims;

using MediatR;

using StruttonTechnologies.Core.Identity.API.Contracts.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Queries
{
    public sealed record GetUserClaimsQuery(string UserId)
    : IRequest<IList<Claim>>, IGetUserClaimsRequest;
}
