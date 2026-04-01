using System.Diagnostics.CodeAnalysis;

using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries
{
    [ExcludeFromCodeCoverage]
    public sealed record GetUserByIdQuery(string UserId)
        : IRequest<UserDetailResult>;
}
