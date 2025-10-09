using MediatR;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record LoginCommand(string Email, string Password) : IRequest<TokenResponseDto>;
}