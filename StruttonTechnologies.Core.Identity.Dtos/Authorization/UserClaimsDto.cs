namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    public record UserClaimsDto(string UserId, IList<ClaimDto> Claims);
}
