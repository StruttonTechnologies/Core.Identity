using MediatR;
using ST.Core.Identity.Dtos.Authentication.User;

namespace ST.Core.Identity.Application.Contracts.Authentication.User.Queries
{
    public record FindByNameQuery(UpdateUsernameRequestDto Dto) : IRequest<UserResponseDto>;
}