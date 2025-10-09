using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record ChangePasswordCommand(string UserId, string CurrentPassword, string NewPassword) : IRequest<IdentityResult>;
}