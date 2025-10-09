using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.ExternalLogins.Commands
{
    public record UnlinkExternalLoginCommand(string UserId, string Provider, string ProviderKey) : IRequest<IdentityResult>;
}