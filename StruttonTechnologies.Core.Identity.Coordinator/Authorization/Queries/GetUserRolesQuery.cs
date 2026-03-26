using MediatR;

using StruttonTechnologies.Core.Identity.API.Contracts.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Queries
{
    public sealed record GetUserRolesQuery(string UserId)
     : IRequest<IList<string>>, IGetUserRolesRequest;
}
