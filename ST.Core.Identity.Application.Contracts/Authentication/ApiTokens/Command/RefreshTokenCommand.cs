using MediatR;
using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Identity.Dtos.Authentication.Tokens;

namespace ST.Core.Identity.Application.Contracts.Authentication.ApiTokens.Commands
{
    /// <summary>
    /// Command to refresh an access token using a valid refresh token.
    /// </summary>
    public record RefreshTokenCommand(RefreshTokenRequestDto Request) : IRequest<LoginResponseDto>;
}
