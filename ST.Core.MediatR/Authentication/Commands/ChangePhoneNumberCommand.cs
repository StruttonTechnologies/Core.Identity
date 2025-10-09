using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record ChangePhoneNumberCommand(string UserId, string PhoneNumber, string Token) : IRequest<IdentityResult>;
}