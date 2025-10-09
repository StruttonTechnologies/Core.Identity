using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Users.Command
{
    public record EnableUserCommand(string UserId) : IRequest<IdentityResult>;
}