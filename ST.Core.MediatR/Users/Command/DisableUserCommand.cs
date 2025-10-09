using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Users.Command
{
    public record DisableUserCommand(string UserId) : IRequest<IdentityResult>;
}