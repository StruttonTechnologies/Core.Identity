using MediatR;
using ST.Core.Identity.Dtos.Authorization;

namespace ST.Core.MediatR.Authorization.Commands
{
    public record GetUserClaimsQuery(string UserId) : IRequest<IList<ClaimDto>>;
}
