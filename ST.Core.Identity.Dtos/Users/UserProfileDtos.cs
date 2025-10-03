namespace ST.Core.Identity.Dtos.Users
{
    public record UserProfileDto(Guid UserId, string UserName, string Email, IList<string> Roles);
}
