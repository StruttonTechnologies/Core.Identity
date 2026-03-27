using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries
{
    public sealed record GetUserRolesQuery(string UserId)
     : IRequest<IList<string>>;
}
