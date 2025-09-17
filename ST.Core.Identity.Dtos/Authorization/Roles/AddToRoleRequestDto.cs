using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    /// <summary>
    /// Represents a request to add a user to a specific role.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <param name="RoleName">The name of the role to add the user to.</param>
    [ExcludeFromCodeCoverage]
    public record AddToRoleRequestDto(string UserId, string RoleName);
}