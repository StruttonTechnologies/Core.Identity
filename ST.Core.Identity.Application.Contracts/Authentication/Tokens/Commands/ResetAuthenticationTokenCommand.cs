using MediatR;
using ST.Core.Identity.Dtos.Authentication.Tokens;

namespace ST.Core.Identity.Application.Contracts.Authentication.Tokens.Commands
{
    public record ResetAuthenticationTokenCommand(GenerateTokenRequestDto Dto) : IRequest<TokenResponseDto>;
}