using MediatR;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands
{
    /// <summary>
    /// Command to log out a user and optionally revoke refresh tokens.
    /// </summary>
    public record LogoutCommand(LogoutRequestDto Request) : IRequest<Unit>;
}