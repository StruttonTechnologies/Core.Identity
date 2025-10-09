using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record ConfirmEmailCommand(string UserId, string Token) : IRequest<IdentityResult>;
}