using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public sealed record ChangePhoneNumberCommand(string UserId, string PhoneNumber, string Token)
     : IRequest<IdentityResult>, IChangePhoneNumberRequest;
}