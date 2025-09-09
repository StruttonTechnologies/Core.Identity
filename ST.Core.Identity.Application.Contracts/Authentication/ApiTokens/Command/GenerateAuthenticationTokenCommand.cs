using MediatR;
using ST.Core.Identity.Dtos.Authentication.Logins;

namespace ST.Core.Identity.Application.Contracts.Authentication.ApiTokens.Commands
{
    /// <summary>
    /// Command to generate an access and refresh token for a user after successful login.
    /// </summary>
    public record GenerateAuthenticationTokenCommand(InternalLoginRequestDto Request) : IRequest<LoginResponseDto>;
}
