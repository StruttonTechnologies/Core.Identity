using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.ExternalLogins;

namespace StruttonTechnologies.Core.Identity.Dtos.Users
{
    [ExcludeFromCodeCoverage]
    public record UserDetailResult(
        string UserId,
        string Email,
        bool EmailConfirmed,
        bool IsActive,
        IReadOnlyCollection<string> Roles,
        IReadOnlyCollection<ClaimDto> Claims,
        IReadOnlyCollection<ExternalLoginInfoDto> Logins
    );
}
