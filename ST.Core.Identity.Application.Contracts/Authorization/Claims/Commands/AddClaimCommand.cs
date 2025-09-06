using MediatR;
using ST.Core.Identity.Dtos.Authorization.Claims;

namespace ST.Core.Identity.Application.Contracts.Authorization.Claims.Commands
{
    public record AddClaimCommand(AddClaimRequestDto Dto) : IRequest<ClaimResponseDto>;
}