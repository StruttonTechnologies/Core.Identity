using MediatR;
using ST.Core.Identity.API.Contracts.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public sealed record ForgotPasswordCommand(string Email)
    : IRequest<string>, IForgotPasswordRequest;
}