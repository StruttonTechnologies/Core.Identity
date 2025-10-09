using MediatR;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.Identity.Dispatch.Users.Queries
{
    public record GetAllUsersQuery : IRequest<IList<UserSummaryDto>>;
}