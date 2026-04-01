using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    [ExcludeFromCodeCoverage]

    public record ClaimsPrincipalDto(
        string? AuthenticationType,
        bool IsAuthenticated,
        string? Name,
        IReadOnlyList<ClaimDto> Claims
    );
}
