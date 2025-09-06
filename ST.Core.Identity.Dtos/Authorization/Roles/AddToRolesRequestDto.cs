namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    public record AddToRolesRequestDto(string UserId, IEnumerable<string> RoleNames);
}