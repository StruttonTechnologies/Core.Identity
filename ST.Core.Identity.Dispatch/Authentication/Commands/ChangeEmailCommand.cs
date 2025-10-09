using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.Authentication;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public sealed record ChangeEmailCommand(string UserId, string NewEmail, string Token)
     : IRequest<IdentityResult>, IChangeEmailRequest;
}