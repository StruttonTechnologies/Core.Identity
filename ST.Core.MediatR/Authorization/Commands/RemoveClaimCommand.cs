using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Authorization.Commands
{
    public record RemoveClaimCommand(string UserId, string ClaimType, string ClaimValue) : IRequest<IdentityResult>;
}