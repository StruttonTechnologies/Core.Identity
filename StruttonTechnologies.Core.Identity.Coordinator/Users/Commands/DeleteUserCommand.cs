using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Commands
{
    public sealed record DeleteUserCommand(string UserId)
    : IRequest<IdentityResult>, IDeleteUserRequest;
}
