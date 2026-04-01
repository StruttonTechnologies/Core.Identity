using System.Diagnostics.CodeAnalysis;

using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries
{
    [ExcludeFromCodeCoverage]
    public sealed record GetNormalizedEmailQuery(string UserId) : IRequest<string?>;
}
