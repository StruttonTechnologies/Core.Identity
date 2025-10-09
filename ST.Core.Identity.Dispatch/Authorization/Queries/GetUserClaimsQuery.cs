using MediatR;
using ST.Core.Identity.API.Contracts.Authorization;
using System.Security.Claims;

namespace ST.Core.Identity.Dispatch.Authorization.Queries
{
    public sealed record GetUserClaimsQuery(string UserId)
    : IRequest<IList<Claim>>, IGetUserClaimsRequest;
}
