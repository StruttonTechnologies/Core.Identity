using MediatR;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<TokenResponseDto>;
}