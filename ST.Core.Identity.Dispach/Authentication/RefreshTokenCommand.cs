using MediatR;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponseDto>;
}