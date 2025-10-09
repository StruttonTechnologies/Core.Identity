using MediatR;

namespace ST.Core.MediatR.Authentication.Commands
{
    public record ForgotPasswordCommand(string Email) : IRequest<string>; // returns reset token or status
}