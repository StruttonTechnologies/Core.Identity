using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Authorization.Commands
{
    public record AssignRoleCommand(string UserId, string RoleName) : IRequest<IdentityResult>;
}