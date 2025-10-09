using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public sealed record ChangePasswordCommand(string UserId, string CurrentPassword, string NewPassword)
     : IRequest<IdentityResult>, IChangePasswordRequest;
}