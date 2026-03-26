using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Authorization;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authorization.Commands
{
    public sealed record RemoveClaimCommand(string UserId, string ClaimType, string ClaimValue)
    : IRequest<IdentityResult>, IRemoveClaimRequest;
}
