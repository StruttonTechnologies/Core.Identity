using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Commands
{
    /// <summary>
    /// Command to create a new user in the system.
    /// </summary>
    /// <param name="UserName">The username for the new user.</param>
    /// <param name="Email">The email address for the new user.</param>
    /// <param name="Password">The password for the new user (optional if using external authentication).</param>
    public sealed record CreateUserCommand(
        string UserName,
        string? Email = null,
        string? Password = null)
        : IRequest<IdentityResult>;
}
