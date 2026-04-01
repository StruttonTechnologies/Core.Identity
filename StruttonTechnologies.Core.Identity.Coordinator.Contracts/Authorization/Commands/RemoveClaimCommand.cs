using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands
{
    [ExcludeFromCodeCoverage]
    public sealed record RemoveClaimCommand(string UserId, string ClaimType, string ClaimValue)
    : IRequest<IdentityResult>;
}
