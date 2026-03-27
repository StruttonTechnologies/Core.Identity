using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.ExternalLogins;

namespace StruttonTechnologies.Core.Identity.Dtos.Users
{
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
