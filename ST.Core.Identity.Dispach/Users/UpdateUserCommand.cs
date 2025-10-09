using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Application.Users
{
    public record UpdateUserCommand(string UserId, string? Email, string? PhoneNumber) : IRequest<IdentityResult>;
}