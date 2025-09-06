using MediatR;
using ST.Core.Identity.Dtos.Authentication.Lockout;

namespace ST.Core.Identity.Application.Contracts.Authentication.Lockout.Queries
{
    public record IsLockedOutQuery(SetLockoutRequestDto Dto) : IRequest<LockoutStatusResponseDto>;
}