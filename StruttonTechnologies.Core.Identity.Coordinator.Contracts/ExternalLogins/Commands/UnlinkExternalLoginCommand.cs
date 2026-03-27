using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands
{
    /// <summary>
    /// Command to unlink an external login provider from a user account.
    /// </summary>
    public sealed record UnlinkExternalLoginCommand(string UserId, string Provider, string ProviderKey)
        : IRequest<IdentityResult>;
}
