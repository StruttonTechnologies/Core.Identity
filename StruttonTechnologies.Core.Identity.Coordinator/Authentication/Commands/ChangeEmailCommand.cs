using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record ChangeEmailCommand(string UserId, string NewEmail, string Token)
     : IRequest<IdentityResult>, IChangeEmailRequest;
}
