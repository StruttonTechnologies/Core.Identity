using MediatR;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.Identity.Dispatch.Users
{
    public record GetUserByIdQuery(string UserId) : IRequest<UserDetailDto>;
}