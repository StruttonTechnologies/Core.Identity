using MediatR;
using ST.Core.Identity.Dtos.Authentication.Tokens;

namespace ST.Core.Identity.Application.Contracts.Authentication.Tokens.Commands
{
    public record ConfirmEmailCommand(ConfirmEmailRequestDto Dto) : IRequest<TokenResponseDto>;
}