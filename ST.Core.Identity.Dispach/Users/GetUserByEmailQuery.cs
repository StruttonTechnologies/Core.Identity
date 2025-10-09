using MediatR;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.Identity.Dispatch.Users
{
    public record GetUserByEmailQuery(string Email) : IRequest<UserDetailDto>;
}