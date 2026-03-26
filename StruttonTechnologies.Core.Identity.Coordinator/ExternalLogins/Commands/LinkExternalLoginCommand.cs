using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.ExternalLogins;

namespace StruttonTechnologies.Core.Identity.Coordinator.ExternalLogins.Commands
{
    public sealed record LinkExternalLoginCommand(string UserId, string Provider, string ProviderKey, string Token)
    : IRequest<IdentityResult>, ILinkExternalLoginRequest;
}
