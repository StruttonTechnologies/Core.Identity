namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    public record ClaimsPrincipalDto(
        string? AuthenticationType,
        bool IsAuthenticated,
        string? Name,
        IReadOnlyList<ClaimDto> Claims
    );
}
