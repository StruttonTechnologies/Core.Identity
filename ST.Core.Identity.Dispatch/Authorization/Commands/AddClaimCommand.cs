using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.Authorization;

namespace ST.Core.Identity.Dispatch.Authorization.Commands
{
    public sealed record AddClaimCommand(string UserId, string ClaimType, string ClaimValue)
     : IRequest<IdentityResult>, IAddClaimRequest;
}