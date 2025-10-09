using MediatR;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponseDto>;
}