using MediatR;

using StruttonTechnologies.Core.Identity.API.Contracts.Users;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Queries
{
    public sealed record GetUserByIdQuery(string UserId)
        : IRequest<UserDetailDto>, IGetUserByIdRequest;
}
