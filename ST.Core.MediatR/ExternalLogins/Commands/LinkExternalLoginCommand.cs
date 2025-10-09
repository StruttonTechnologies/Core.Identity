using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.ExternalLogins.Commands
{
    public record LinkExternalLoginCommand(string UserId, string Provider, string ProviderKey, string Token) : IRequest<IdentityResult>;
}