using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.ExternalLogins;

namespace ST.Core.Identity.Dispatch.ExternalLogins.Commands
{
    public sealed record LinkExternalLoginCommand(string UserId, string Provider, string ProviderKey, string Token)
    : IRequest<IdentityResult>, ILinkExternalLoginRequest;
}