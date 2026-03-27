using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries
{
    public sealed record GetNormalizedEmailQuery(string UserId) : IRequest<string?>;
}
