using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Commands
{
    public sealed record RemoveRoleCommand(string UserId, string RoleName)
    : IRequest<IdentityResult>, IRemoveRoleRequest;
}
