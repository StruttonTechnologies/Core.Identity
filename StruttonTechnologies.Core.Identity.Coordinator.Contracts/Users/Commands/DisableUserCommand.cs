using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands
{
    /// <summary>
    /// Command to disable a user account.
    /// </summary>
    public sealed record DisableUserCommand(string UserId) : IRequest<IdentityResult>;
}
