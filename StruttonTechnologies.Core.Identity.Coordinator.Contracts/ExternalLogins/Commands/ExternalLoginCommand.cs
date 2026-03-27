using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands
{
    public sealed record ExternalLoginCommand(string Provider, string IdToken)
    : IRequest<TokenResponseDto>;
}
