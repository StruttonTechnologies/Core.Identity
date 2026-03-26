using MediatR;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Queries
{
    public sealed record GetNormalizedEmailQuery(string UserId) : IRequest<string?>;
}
