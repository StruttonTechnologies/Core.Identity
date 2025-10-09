using MediatR;
using ST.Core.Identity.API.Contracts.Users;
using ST.Core.Identity.Dtos.Users;

namespace ST.Core.Identity.Dispatch.Users.Queries
{
    public sealed record GetUserByEmailQuery(string Email)
     : IRequest<UserDetailDto>, IGetUserByEmailRequest;
}