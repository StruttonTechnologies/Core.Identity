namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    /// <summary>
    /// Represents the data required to replace an existing claim for a user.
    /// </summary>
    public record ReplaceClaimRequestDto(Guid UserId, string ClaimType, string OldValue, string NewValue);
}