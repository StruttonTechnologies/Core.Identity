using MediatR;
using ST.Core.Identity.Dtos.Authentication.Logins;

namespace ST.Core.Identity.Application.Contracts.Authentication.Logins.Queries
{
    public record GetLoginsQuery(AddLoginRequestDto Dto) : IRequest<IEnumerable<LoginInfoResponseDto>>;
}