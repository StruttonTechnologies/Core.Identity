using System.Diagnostics.CodeAnalysis;

using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    [ExcludeFromCodeCoverage]
    public sealed record ForgotPasswordCommand(string Email)
    : IRequest<string>;
}
