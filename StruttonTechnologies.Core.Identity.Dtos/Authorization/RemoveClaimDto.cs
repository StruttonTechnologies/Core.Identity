namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    public record RemoveClaimDto(string UserId, string ClaimType, string ClaimValue);
}
