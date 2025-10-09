using MediatR;
using ST.Core.Identity.API.Contracts.Authorization;

namespace ST.Core.Identity.Dispatch.Authorization.Queries
{
    public sealed record GetUserRolesQuery(string UserId)
     : IRequest<IList<string>>, IGetUserRolesRequest;
}