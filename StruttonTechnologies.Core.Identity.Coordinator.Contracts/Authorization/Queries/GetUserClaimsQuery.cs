using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries
{
    [ExcludeFromCodeCoverage]
    public sealed record GetUserClaimsQuery(string UserId)
    : IRequest<IList<Claim>>;
}
