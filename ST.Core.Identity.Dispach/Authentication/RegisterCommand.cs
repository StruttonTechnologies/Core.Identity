using MediatR;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record RegisterCommand(string Email, string Password) : IRequest<string>; // returns UserId or confirmation token
}