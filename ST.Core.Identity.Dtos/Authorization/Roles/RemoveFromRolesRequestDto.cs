namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    /// <summary>
    /// Represents a request to remove a user from specified roles.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <param name="RoleNames">A collection of role names to remove the user from.</param>
    public record RemoveFromRolesRequestDto(string UserId, IEnumerable<string> RoleNames);
}