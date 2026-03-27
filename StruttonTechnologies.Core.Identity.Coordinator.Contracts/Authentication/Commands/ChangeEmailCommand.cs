using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    public sealed record ChangeEmailCommand(string UserId, string NewEmail, string Token)
     : IRequest<IdentityResult>;
}
