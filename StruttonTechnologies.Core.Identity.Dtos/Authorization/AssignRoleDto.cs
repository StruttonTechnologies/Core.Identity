using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    [ExcludeFromCodeCoverage]

    public record AssignRoleDto(string UserId, string RoleName);
}
