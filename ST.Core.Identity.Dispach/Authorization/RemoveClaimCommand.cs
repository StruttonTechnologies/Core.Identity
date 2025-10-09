using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Application.Authorization
{
    public record RemoveClaimCommand(string UserId, string ClaimType, string ClaimValue) : IRequest<IdentityResult>;
}