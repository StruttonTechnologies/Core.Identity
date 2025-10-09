using MediatR;

namespace ST.Core.Identity.Dispatch.Users.Queries
{
    public sealed record GetNormalizedEmailQuery(string UserId) : IRequest<string?>;
}

