using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispatch.ExternalLogins
{
    public record LinkExternalLoginCommand(string UserId, string Provider, string ProviderKey, string Token) : IRequest<IdentityResult>;
}