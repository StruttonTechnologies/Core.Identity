using MediatR;

namespace ST.Core.Identity.Dispatch.Authorization
{
    public record GetUserRolesQuery(string UserId) : IRequest<IList<string>>;
}