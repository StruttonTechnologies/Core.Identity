using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Users.Command
{
    public record DeleteUserCommand(string UserId) : IRequest<IdentityResult>;
}