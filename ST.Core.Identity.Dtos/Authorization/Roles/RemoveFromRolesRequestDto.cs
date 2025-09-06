namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    public record RemoveFromRolesRequestDto(string UserId, IEnumerable<string> RoleNames);
}