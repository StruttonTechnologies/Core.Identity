using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record ChangePasswordCommand(string UserId, string CurrentPassword, string NewPassword)
     : IRequest<IdentityResult>, IChangePasswordRequest;
}
