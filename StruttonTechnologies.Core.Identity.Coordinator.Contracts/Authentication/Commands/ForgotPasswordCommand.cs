using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    public sealed record ForgotPasswordCommand(string Email)
    : IRequest<string>;
}
