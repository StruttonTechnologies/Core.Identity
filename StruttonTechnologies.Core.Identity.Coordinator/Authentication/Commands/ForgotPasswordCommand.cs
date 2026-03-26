using MediatR;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record ForgotPasswordCommand(string Email)
    : IRequest<string>, IForgotPasswordRequest;
}
