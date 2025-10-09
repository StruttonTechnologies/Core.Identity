using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record ChangeEmailCommand(string UserId, string NewEmail, string Token) : IRequest<IdentityResult>;
}