using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Commands
{
    public sealed record UpdateUserCommand(string UserId, string? Email, string? PhoneNumber)
    : IRequest<IdentityResult>, IUpdateUserRequest;
}
