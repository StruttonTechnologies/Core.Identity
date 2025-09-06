using MediatR;
using ST.Core.Identity.Dtos.Authentication.TwoFactor;

namespace ST.Core.Identity.Application.Contracts.Authentication.TwoFactor.Commands
{
    public record SetTwoFactorEnabledCommand(SetTwoFactorEnabledRequestDto Dto) : IRequest<TwoFactorStatusResponseDto>;
}