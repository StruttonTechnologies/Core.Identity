namespace ST.Core.Identity.Dtos.Authentication.Password
{
    public record ChangePasswordRequestDto(string UserId, string OldPassword, string NewPassword);
}