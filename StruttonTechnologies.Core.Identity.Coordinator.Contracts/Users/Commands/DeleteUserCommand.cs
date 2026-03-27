using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands
{
    public sealed record DeleteUserCommand(string UserId)
    : IRequest<IdentityResult>;
}
