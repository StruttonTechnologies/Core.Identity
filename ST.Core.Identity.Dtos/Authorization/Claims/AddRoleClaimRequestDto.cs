using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Represents the data required to add a claim to a role.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record AddRoleClaimRequestDto(string RoleName, string ClaimType, string ClaimValue);
}
