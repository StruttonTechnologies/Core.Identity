using System.Diagnostics.CodeAnalysis;

using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries
{
    [ExcludeFromCodeCoverage]
    public sealed record GetUserRolesQuery(string UserId)
     : IRequest<IList<string>>;
}
