using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    [ExcludeFromCodeCoverage]

    public record IdentityDto(
        string? AuthenticationType,
        bool IsAuthenticated,
        string? Name,
        IReadOnlyList<ClaimDto> Claims
    );
}
