using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispatch.Users
{
    public record DisableUserCommand(string UserId) : IRequest<IdentityResult>;
}