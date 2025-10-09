using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispatch.Authorization
{
    public record AssignRoleCommand(string UserId, string RoleName) : IRequest<IdentityResult>;
}