using System.Diagnostics.CodeAnalysis;

using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    [ExcludeFromCodeCoverage]
    public record AuthenticationCommand(string Email, string Password)
        : IRequest<TokenResponseDto>;
}
