using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    public sealed record ChangePhoneNumberCommand(string UserId, string PhoneNumber, string Token)
     : IRequest<IdentityResult>;
}
