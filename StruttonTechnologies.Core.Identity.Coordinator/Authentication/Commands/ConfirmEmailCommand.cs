using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record ConfirmEmailCommand(string UserId, string Token)
    : IRequest<IdentityResult>, IConfirmEmailRequest;
}
