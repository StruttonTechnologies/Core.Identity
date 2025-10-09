using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispatch.ExternalLogins
{
    public record UnlinkExternalLoginCommand(string UserId, string Provider, string ProviderKey) : IRequest<IdentityResult>;
}