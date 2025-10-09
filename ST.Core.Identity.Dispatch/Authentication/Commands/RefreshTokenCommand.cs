using MediatR;
using ST.Core.Identity.API.Contracts.Authentication;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<TokenResponseDto>, IRefreshTokenRequest;

}