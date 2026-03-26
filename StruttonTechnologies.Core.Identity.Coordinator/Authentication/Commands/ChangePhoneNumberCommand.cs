using MediatR;

using Microsoft.AspNetCore.Identity;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record ChangePhoneNumberCommand(string UserId, string PhoneNumber, string Token)
     : IRequest<IdentityResult>, IChangePhoneNumberRequest;
}
