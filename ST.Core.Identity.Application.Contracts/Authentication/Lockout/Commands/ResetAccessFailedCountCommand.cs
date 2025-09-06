using MediatR;
using ST.Core.Identity.Dtos.Authentication.Lockout;

namespace ST.Core.Identity.Application.Contracts.Authentication.Lockout.Commands
{
    public record ResetAccessFailedCountCommand(SetLockoutRequestDto Dto) : IRequest<LockoutStatusResponseDto>;
}