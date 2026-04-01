using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Users
{
    [ExcludeFromCodeCoverage]
    public record UserSummaryResult(
    string UserId,
    string Email,
    string DisplayName,
    bool EmailConfirmed,
    bool IsActive);
}
