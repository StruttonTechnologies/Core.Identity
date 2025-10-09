using ST.Core.Identity.Dtos.Authorization;
using ST.Core.Identity.Dtos.ExternalLogins;

namespace ST.Core.Identity.Dtos.Users
{
    public record UserSummaryDto(string UserId, string Email, bool EmailConfirmed, bool IsActive);

    public record UserDetailDto(
        string UserId,
        string Email,
        bool EmailConfirmed,
        bool IsActive,
        IList<string> Roles,
        IList<ClaimDto> Claims,
        IList<ExternalLoginInfoDto> Logins
    );
}