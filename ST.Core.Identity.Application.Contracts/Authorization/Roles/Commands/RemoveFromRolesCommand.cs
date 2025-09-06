using MediatR;
using ST.Core.Identity.Dtos.Authorization.Roles;

namespace ST.Core.Identity.Application.Contracts.Authorization.Roles.Commands
{
    public record RemoveFromRolesCommand(RemoveFromRolesRequestDto Dto) : IRequest<RoleAssignmentResponseDto>;
}