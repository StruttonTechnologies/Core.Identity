namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Represents the data required to add a claim to a role.
    /// </summary>
    public record AddRoleClaimRequestDto(string RoleName, string ClaimType, string ClaimValue);
}
