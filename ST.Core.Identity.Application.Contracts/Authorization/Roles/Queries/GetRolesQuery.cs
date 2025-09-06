using MediatR;
using ST.Core.Identity.Dtos.Authorization.Roles;

namespace ST.Core.Identity.Application.Contracts.Authorization.Roles.Queries
{
    public record GetRolesQuery(AddToRoleRequestDto Dto) : IRequest<RoleAssignmentResponseDto>;
}