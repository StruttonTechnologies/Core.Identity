namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    public record AddClaimDto(string UserId, string ClaimType, string ClaimValue);
}
