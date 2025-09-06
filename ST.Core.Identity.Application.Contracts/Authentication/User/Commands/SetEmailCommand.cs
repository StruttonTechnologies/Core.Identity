using MediatR;
using ST.Core.Identity.Dtos.Authentication.User;

namespace ST.Core.Identity.Application.Contracts.Authentication.User.Commands
{
    public record SetEmailCommand(UpdateEmailRequestDto Dto) : IRequest<UserResponseDto>;
}