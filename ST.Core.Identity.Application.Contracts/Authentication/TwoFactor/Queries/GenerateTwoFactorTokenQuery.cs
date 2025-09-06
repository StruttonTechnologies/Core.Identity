using MediatR;
using ST.Core.Identity.Dtos.Authentication.TwoFactor;

namespace ST.Core.Identity.Application.Contracts.Authentication.TwoFactor.Queries
{
    public record GenerateTwoFactorTokenQuery(GenerateTwoFactorTokenRequestDto Dto) : IRequest<TwoFactorStatusResponseDto>;
}