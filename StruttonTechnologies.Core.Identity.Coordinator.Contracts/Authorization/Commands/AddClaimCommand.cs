using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands
{
    /// <summary>
    /// Command to add a claim to a user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record AddClaimCommand(string UserId, string ClaimType, string ClaimValue)
        : IRequest<IdentityResult>;
}
