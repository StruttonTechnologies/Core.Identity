using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispatch.Authorization
{
    public record AddClaimCommand(string UserId, string ClaimType, string ClaimValue) : IRequest<IdentityResult>;
}