namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    public record RoleAssignmentResponseDto(string UserId, IEnumerable<string> Roles);
}