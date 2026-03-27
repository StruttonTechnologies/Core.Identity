namespace StruttonTechnologies.Core.Identity.Dtos.Users
{
    public record UserProfileResult(
        string UserId,
        string Email,
        string DisplayName,
        bool IsLockedOut
    );
}
