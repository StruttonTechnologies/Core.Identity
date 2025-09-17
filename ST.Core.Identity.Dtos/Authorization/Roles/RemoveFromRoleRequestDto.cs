namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    /// <summary>
    /// Represents a request to remove a user from a specific role.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user to be removed from the role.</param>
    /// <param name="RoleName">The name of the role from which the user will be removed.</param>
    public record RemoveFromRoleRequestDto(string UserId, string RoleName);
}