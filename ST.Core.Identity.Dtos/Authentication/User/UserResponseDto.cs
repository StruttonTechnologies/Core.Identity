namespace ST.Core.Identity.Dtos.Authentication.User
{
    public record UserResponseDto(string UserId, string UserName, string Email, bool EmailConfirmed);
}