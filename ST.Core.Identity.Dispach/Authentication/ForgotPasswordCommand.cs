using MediatR;

namespace ST.Core.Identity.Dispach.Authentication
{
    public record ForgotPasswordCommand(string Email) : IRequest<string>; // returns reset token or status
}