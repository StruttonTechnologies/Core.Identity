using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Request DTO for retrieving claims associated with a specific role.
    /// </summary>
    /// <param name="RoleName">The name of the role for which claims are requested.</param>
    [ExcludeFromCodeCoverage]
    public record GetRoleClaimsRequestDto(string RoleName);
}