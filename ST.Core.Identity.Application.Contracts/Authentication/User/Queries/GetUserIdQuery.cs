using MediatR;
using ST.Core.Identity.Dtos.Authentication.User;

namespace ST.Core.Identity.Application.Contracts.Authentication.User.Queries
{
    public record GetUserIdQuery(DeleteUserRequestDto Dto) : IRequest<UserResponseDto>;
}