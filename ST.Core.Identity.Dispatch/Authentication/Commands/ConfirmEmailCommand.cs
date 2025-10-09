using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public sealed record ConfirmEmailCommand(string UserId, string Token)
    : IRequest<IdentityResult>, IConfirmEmailRequest;
}