using MediatR;

using StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record ExternalLoginCommand(string Provider, string IdToken) : IRequest<TokenResponseDto>, IExternalLoginRequest;
}
