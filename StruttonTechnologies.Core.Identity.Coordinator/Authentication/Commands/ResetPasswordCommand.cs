using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record ResetPasswordCommand(string UserId, string Token, string NewPassword)
    : IRequest<IdentityResult>, IResetPasswordRequest;
}
