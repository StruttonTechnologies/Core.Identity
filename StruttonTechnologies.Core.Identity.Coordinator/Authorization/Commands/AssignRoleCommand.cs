using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Commands
{
    /// <summary>
    /// Command to assign a role to a user.
    /// </summary>
    public sealed record AssignRoleCommand(string UserId, string RoleName)
        : IRequest<IdentityResult>;
}
