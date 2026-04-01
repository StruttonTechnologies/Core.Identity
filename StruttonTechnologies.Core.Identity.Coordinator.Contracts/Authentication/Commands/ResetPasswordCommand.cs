using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    [ExcludeFromCodeCoverage]
    public sealed record ResetPasswordCommand(string UserId, string Token, string NewPassword)
    : IRequest<IdentityResult>;
}
