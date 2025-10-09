using MediatR;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.MediatR.Users.Queiries
{
    public record GetUserByIdQuery(string UserId) : IRequest<UserDetailDto>;
}