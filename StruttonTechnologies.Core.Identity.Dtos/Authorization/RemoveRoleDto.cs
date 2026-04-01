using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    [ExcludeFromCodeCoverage]

    public record RemoveRoleDto(string UserId, string RoleName);
}
