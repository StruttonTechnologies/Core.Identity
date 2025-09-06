using MediatR;
using ST.Core.Identity.Dtos.Authorization.Claims;

namespace ST.Core.Identity.Application.Contracts.Authorization.Claims.Queries
{
    public record GetUserClaimsQuery(GetUserClaimsRequestDto Dto) : IRequest<IEnumerable<ClaimResponseDto>>;
}