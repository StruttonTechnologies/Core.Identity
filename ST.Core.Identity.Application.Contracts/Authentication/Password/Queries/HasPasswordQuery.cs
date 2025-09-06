using MediatR;
using ST.Core.Identity.Dtos.Authentication.Password;

namespace ST.Core.Identity.Application.Contracts.Authentication.Password.Queries
{
    public record HasPasswordQuery(RemovePasswordRequestDto Dto) : IRequest<PasswordStatusResponseDto>;
}