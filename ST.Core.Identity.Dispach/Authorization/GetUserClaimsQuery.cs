using MediatR;
using ST.Core.Identity.Dtos.Authorization;

namespace ST.Core.Identity.Dispatch.Authorization
{
    public record GetUserClaimsQuery(string UserId) : IRequest<IList<ClaimDto>>;
}
