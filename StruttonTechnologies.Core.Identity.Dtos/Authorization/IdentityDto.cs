namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    public record IdentityDto(
        string? AuthenticationType,
        bool IsAuthenticated,
        string? Name,
        IReadOnlyList<ClaimDto> Claims
    );
}
