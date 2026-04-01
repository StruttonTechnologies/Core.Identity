using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    [ExcludeFromCodeCoverage]

    public record RemoveClaimDto(string UserId, string ClaimType, string ClaimValue);
}
