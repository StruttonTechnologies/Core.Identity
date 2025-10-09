using MediatR;
using ST.Core.Identity.API.Contracts.ExternalLogins;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.ExternalLogins.Commands
{
    public sealed record ExternalLoginCommand(string Provider, string IdToken)
    : IRequest<TokenResponseDto>, IExternalLoginRequest;
}
