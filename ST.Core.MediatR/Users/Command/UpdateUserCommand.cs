using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Users.Command
{
    public record UpdateUserCommand(string UserId, string? Email, string? PhoneNumber) : IRequest<IdentityResult>;
}