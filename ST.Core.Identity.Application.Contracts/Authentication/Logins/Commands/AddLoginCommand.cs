using MediatR;
using ST.Core.Identity.Dtos.Authentication.Logins;

namespace ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands
{
    public record AddLoginCommand(AddLoginRequestDto Dto) : IRequest<LoginInfoResponseDto>;
}