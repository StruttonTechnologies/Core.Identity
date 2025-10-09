using MediatR;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record RegisterCommand(string Email, string Password) : IRequest<string>; // returns UserId or confirmation token
}