using MediatR;
using ST.Core.Identity.Dtos.Authentication.Tokens;

namespace ST.Core.Identity.Application.Contracts.Authentication.Tokens.Queries
{
    public record GenerateEmailConfirmationTokenQuery(GenerateTokenRequestDto Dto) : IRequest<TokenResponseDto>;
}