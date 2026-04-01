using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands
{
    /// <summary>
    /// Command to assign a role to a user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record AssignRoleCommand(string UserId, string RoleName)
        : IRequest<IdentityResult>;
}
