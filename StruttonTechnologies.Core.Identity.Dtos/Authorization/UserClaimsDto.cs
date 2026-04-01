using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    [ExcludeFromCodeCoverage]

    public record UserClaimsDto(string UserId, IList<ClaimDto> Claims);
}
