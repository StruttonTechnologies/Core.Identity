using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    [ExcludeFromCodeCoverage]
    public sealed record ChangePhoneNumberCommand(string UserId, string PhoneNumber, string Token)
     : IRequest<IdentityResult>;
}
