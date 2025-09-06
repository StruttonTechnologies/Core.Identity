namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Represents the data required to remove a claim from a user.
    /// </summary>
    public record RemoveClaimRequestDto(Guid UserId, string ClaimType, string ClaimValue);
}