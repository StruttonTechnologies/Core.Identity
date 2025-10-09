using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispatch.Authorization
{
    public record RemoveRoleCommand(string UserId, string RoleName) : IRequest<IdentityResult>;
}