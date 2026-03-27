using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands
{
    public sealed record RemoveRoleCommand(string UserId, string RoleName)
    : IRequest<IdentityResult>;
}
