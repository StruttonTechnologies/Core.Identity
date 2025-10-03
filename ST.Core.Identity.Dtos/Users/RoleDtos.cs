namespace ST.Core.Identity.Dtos.Users
{
    public record RoleDto(string RoleName);

    public record UserRoleAssignmentDto(Guid UserId, string RoleName);
}