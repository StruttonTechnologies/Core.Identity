using MediatR;
using ST.Core.Identity.Dtos.Authentication.Logins;

namespace ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands
{
    public record RemoveLoginCommand(RemoveLoginRequestDto Dto) : IRequest<LoginInfoResponseDto>;
}