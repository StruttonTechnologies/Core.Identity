using MediatR;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.MediatR.Users.Queiries
{
    public record GetUserByEmailQuery(string Email) : IRequest<UserDetailDto>;
}