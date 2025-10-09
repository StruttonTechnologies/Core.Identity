using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record ChangeEmailCommand(string UserId, string NewEmail, string Token) : IRequest<IdentityResult>;
}