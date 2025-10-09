using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.Users;

namespace ST.Core.Identity.Dispatch.Users.Commands
{
    public sealed record UpdateUserCommand(string UserId, string? Email, string? PhoneNumber)
    : IRequest<IdentityResult>, IUpdateUserRequest;
}