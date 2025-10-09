using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record ConfirmEmailCommand(string UserId, string Token) : IRequest<IdentityResult>;
}