using MediatR;
using ST.Core.Identity.API.Contracts.Users;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.Identity.Dispatch.Users.Queries
{
    public sealed record GetUserByIdQuery(string UserId)
        : IRequest<UserDetailDto>, IGetUserByIdRequest;
}