namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    /// <summary>
    /// Represents the response for a role assignment operation, containing the user ID and assigned roles.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <param name="Roles">The collection of roles assigned to the user.</param>
    public record RoleAssignmentResponseDto(string UserId, IEnumerable<string> Roles);
}