using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Application.Users
{
    public record EnableUserCommand(string UserId) : IRequest<IdentityResult>;
}