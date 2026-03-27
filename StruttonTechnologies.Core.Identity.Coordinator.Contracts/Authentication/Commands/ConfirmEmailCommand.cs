using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    public sealed record ConfirmEmailCommand(string UserId, string Token)
    : IRequest<IdentityResult>;
}
