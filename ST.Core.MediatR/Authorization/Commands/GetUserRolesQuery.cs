using MediatR;

namespace ST.Core.MediatR.Authorization.Commands
{
    public record GetUserRolesQuery(string UserId) : IRequest<IList<string>>;
}