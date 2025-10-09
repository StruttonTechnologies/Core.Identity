using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record ChangePhoneNumberCommand(string UserId, string PhoneNumber, string Token) : IRequest<IdentityResult>;
}