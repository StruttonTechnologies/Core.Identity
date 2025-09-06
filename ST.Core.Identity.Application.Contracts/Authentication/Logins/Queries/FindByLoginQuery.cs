using MediatR;
using ST.Core.Identity.Dtos.Authentication.Logins;

namespace ST.Core.Identity.Application.Contracts.Authentication.Logins.Queries
{
    public record FindByLoginQuery(AddLoginRequestDto Dto) : IRequest<LoginInfoResponseDto>;
}