using MediatR;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands
{
    /// <summary>
    /// Command to authenticate a user via internal credentials.
    /// </summary>
    public record InternalLoginCommand(InternalLoginRequestDto Request) : IRequest<LoginResponseDto>;
}