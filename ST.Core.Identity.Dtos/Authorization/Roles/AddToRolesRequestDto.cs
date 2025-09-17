using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authorization.Roles
{
    /// <summary>
    /// Represents a request to add a user to one or more roles.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <param name="RoleNames">A collection of role names to which the user will be added.</param>
    [ExcludeFromCodeCoverage]
    public record AddToRolesRequestDto(string UserId, IEnumerable<string> RoleNames);
}