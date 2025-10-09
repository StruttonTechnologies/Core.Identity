using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record ChangePasswordCommand(string UserId, string CurrentPassword, string NewPassword) : IRequest<IdentityResult>;
}