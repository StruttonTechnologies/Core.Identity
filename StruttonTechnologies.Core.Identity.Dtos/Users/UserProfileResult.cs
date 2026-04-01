using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Users
{
    [ExcludeFromCodeCoverage]
    public record UserProfileResult(
        string UserId,
        string Email,
        string DisplayName,
        bool IsLockedOut
    );
}
