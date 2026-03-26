using MediatR;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public record AuthenticationCommand(string Email, string Password)
        : IRequest<TokenResponseDto>, ILoginRequest;
}
