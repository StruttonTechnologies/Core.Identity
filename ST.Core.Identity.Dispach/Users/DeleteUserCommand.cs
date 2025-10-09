using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispatch.Users
{
    public record DeleteUserCommand(string UserId) : IRequest<IdentityResult>;
}