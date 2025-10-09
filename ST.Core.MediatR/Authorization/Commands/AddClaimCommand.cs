using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Authorization.Commands
{
    public record AddClaimCommand(string UserId, string ClaimType, string ClaimValue) : IRequest<IdentityResult>;
}