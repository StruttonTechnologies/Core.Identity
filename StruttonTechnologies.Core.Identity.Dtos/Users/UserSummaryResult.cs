namespace StruttonTechnologies.Core.Identity.Dtos.Users
{
    public record UserSummaryResult(
    string UserId,
    string Email,
    string DisplayName,
    bool EmailConfirmed,
    bool IsActive);
}
