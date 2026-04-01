using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Commands
{
    /// <summary>
    /// Command to enable a user account by removing lockout.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record EnableUserCommand(string UserId) : IRequest<IdentityResult>;
}
