using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Commands
{
    /// <summary>
    /// Command to enable a user account by removing lockout.
    /// </summary>
    public sealed record EnableUserCommand(string UserId) : IRequest<IdentityResult>;
}
