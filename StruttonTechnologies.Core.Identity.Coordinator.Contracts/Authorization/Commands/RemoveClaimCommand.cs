using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Commands
{
    public sealed record RemoveClaimCommand(string UserId, string ClaimType, string ClaimValue)
    : IRequest<IdentityResult>;
}
